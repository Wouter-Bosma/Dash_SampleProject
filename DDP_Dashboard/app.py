# install dash, dash-bootstrap-components, plotly, pandas, scikit-learn
from dash import Dash, html, Input, Output, State, callback_context
from InputDataSelection import DowConstituents, Intervals
from dash.exceptions import PreventUpdate
import yfinance as yf
import plotly.graph_objs as go
import pandas as pd
import uuid
import os
from TopLevelLayout import *

from sklearn import linear_model

app = Dash(
    __name__,
    suppress_callback_exceptions=True,
    external_stylesheets=[dbc.themes.SPACELAB, dbc.icons.FONT_AWESOME],

)

baseGraph = dcc.Graph(id="baseData", className="pb-4")

app.layout = dbc.Container(
    [
        dbc.Row(
            dbc.Col(
                html.H2(
                    "Data Visualization And Interoperability POC",
                    className="text-center bg-primary text-white p-2",
                )
            )
        ),
        dbc.Row(
            [
                dbc.Col(primaryTabs, className="mt-4 border")
            ]
        ),
        dbc.Row(dbc.Col(disclaimer)),
        dcc.Store(id="storageInput", storage_type="session", data={}),
        dcc.Store(id="storageOutput", storage_type="session", data={}),
    ],
    fluid=True,
)

def linechart(dff, instrument, interval, xaxe, yaxe, label, customtitle = None):
    fig = go.Figure()
    fig.add_trace(
        go.Scatter(
            x=dff[xaxe],
            y=dff[yaxe],
            name=label,
            mode='markers'
        )
    )

    if customtitle is None:
        customtitle = label + " prices for " + instrument + " in " + interval + " interval"

    fig.update_layout(
        title= customtitle,
        template="none",
        showlegend=True,
    )
    return fig


def formatdatetimestring(input):
    input = str(input)
    if input.count('-') == 3:
        return input[0:input.rfind('-')]
    else:
        return input

@app.callback(
    Output("storageOutput", "data"),
    Input("domainlanguagebutton", "n_clicks"),
    State("storageInput", "data"),
    State("domainLanguageInput", "value"),
    State("defaultLanguageInput", "value"),
    prevent_initiall_call=True
)
def processDomainLanguage(n_clicks, inputData, userDomainLanguage, defaultDomainLanguage):
    if inputData is None or n_clicks == 0:
        raise PreventUpdate
    dff = pd.DataFrame(inputData)
    uid = str(uuid.uuid4());
    dff.to_csv(uid + 'localDataOutput.csv', index=False)
    alphaFile = open(uid + 'alphaFile.txt', 'w')
    alphaFile.writelines(userDomainLanguage)
    alphaFile.write('\n')
    alphaFile.writelines(defaultDomainLanguage)
    alphaFile.close()

    commandToRun = 'debug\\net6.0\\Alpha.exe ' + uid + 'alphaFile.txt ' + uid + 'localDataOutput.csv ' + uid + 'output.csv'
    os.system(commandToRun)
    dff = pd.read_csv(uid + 'output.csv', sep=';')

    os.remove(uid + 'localDataOutput.csv')
    os.remove(uid + 'alphaFile.txt')
    os.remove(uid + 'output.csv')
    return dff.to_dict("records")


@app.callback(
    Output("storageInput", "data"),
    Input("dataselectionbutton", "n_clicks"),
    State("baseinstrument-selection", "value"),
    State("interval", "value"),
    prevent_initiall_call=True
)
def readData(n_clicks, instrument, intervalChosen):
    if n_clicks is None or n_clicks == 0:
        raise PreventUpdate
    chosenInterval = Intervals[intervalChosen]['yahooterm']
    availablePeriod = Intervals[intervalChosen]['yahooperiod']
    result = yf.download(instrument, interval=chosenInterval, period=availablePeriod)
    result = result.reset_index()
    result.rename(columns={"index": "Datetime", "Date": "Datetime"}, inplace=True)
    result["Datetime"] = result["Datetime"].apply(formatdatetimestring)
    return result.to_dict("records")

def PlotEstimateTarget(alphas, trainingPerc, settings, optim, inputData):
    if not (inputData is None or inputData == {}):
        dff = pd.DataFrame(inputData)
        dff['Datetime'] = pd.to_datetime(dff['Datetime'])

        xs = []
        for selectedAlpha in alphas:
            stddev = dff[selectedAlpha].std()
            mean = dff[selectedAlpha].mean()
            dff[selectedAlpha + 'norm'] = (dff[selectedAlpha] - mean) / stddev
            xs.append(selectedAlpha + 'norm')
        if optim == 'return':
            dff['TARGET'] = (dff['CLOSETMR'] - dff['CLOSE'])/dff['CLOSE']
        else:
            dff['TARGET'] = dff['CLOSETMR'] - dff['CLOSE']

        length = len(dff)
        window = int(length * trainingPerc / 100)
        dff['ESTIMATE'] = 0
        for start in range(0, (length-window)):
            x = dff[xs].iloc[start:(start + window)]
            y = dff['TARGET'].iloc[start:(start + window)]
            regr = linear_model.LinearRegression()
            regr.fit(x, y)
            new_x = dff[xs].iloc[start+window:start + window + 1]
            dff.loc[start+window, "ESTIMATE"] = regr.predict(new_x)

        fig = go.Figure()
        fig.add_trace(
            go.Scatter(
                x=dff["TARGET"],
                y=dff["ESTIMATE"],
                name="Prediction",
                mode='markers'
            )
        )
        return fig
    return None

@app.callback(
    Output("mainGraphs", "children"),
    Output("axeSelections", "children"),
    Input("dataselectionbutton", "n_clicks"),
    Input("storageInput", "data"),
    Input("storageOutput", "data"),
    Input("secondaryTabsSelection", "active_tab"),
    State("alphadropdown", "value"),
    prevent_initiall_call=True,
)
def returnBaseGraph(n_clicks, inputData, outputData, tabSelection, alphadropdown):
    if tabSelection == 'tabs1':
        if not (inputData is None or inputData == {}):
            dff = pd.DataFrame(inputData)
            dff['Datetime'] = pd.to_datetime(dff['Datetime'])
            columns = dff.columns
            return [baseGraph, [dbc.Col([html.H6("X Axe selection"), dcc.Dropdown(columns, columns[0], id="xaxe")]),
                                dbc.Col([html.H6("Y Axe selection"), dcc.Dropdown(columns, columns[1], id="yaxe")])]]
    if tabSelection == 'tabs2':
        if not (outputData is None or outputData == {}):
            dff = pd.DataFrame(outputData)
            dff['Datetime'] = pd.to_datetime(dff['Datetime'])
            columns = dff.columns
            return [baseGraph, [dbc.Col([html.H6("X Axe selection"), dcc.Dropdown(columns, columns[0], id="xaxe")]),
                                dbc.Col([html.H6("Y Axe selection"), dcc.Dropdown(columns, columns[1], id="yaxe")])]]
    if tabSelection == 'tabs3':
        return [baseGraph, [dbc.Col([html.H6("X Axe selection"), dcc.Dropdown(['TARGET'], 'TARGET', id="xaxe")]),
                                dbc.Col([html.H6("Y Axe selection"), dcc.Dropdown(['ESTIMATE'], 'ESTIMATE', id="yaxe")])]]
    return [None, None]


@app.callback(
    Output("baseData", "figure"),
    Input("alphaselectionbutton", "n_clicks"),
    Input("storageInput", "data"),
    Input("storageOutput", "data"),
    Input("secondaryTabsSelection", "active_tab"),
    Input("xaxe", "value"),
    Input("yaxe", "value"),
    State("baseinstrument-selection", "value"),
    State("interval", "value"),
    Input("alphadropdown", "value"),
    State("trainingperc", "value"),
    State("normalizationsettings", "value"),
    State("optimization", "value"),
)
def plotBaseDataGraph(n_clicks, inputData, outputData, tabSelection, xaxe, yaxe, instrumentName, interval, alphas, trainingPerc, settings, optim):
    if tabSelection == 'tabs1':
        instrumentlongname = instrumentName
        for constituent in DowConstituents:
            if constituent['value'] == instrumentName:
                instrumentlongname = constituent['label']
                break
        if inputData is None or inputData == {}:
            raise PreventUpdate
        dff = pd.DataFrame(inputData)
        dff['Datetime'] = pd.to_datetime(dff['Datetime'])
        return linechart(dff, instrumentlongname, Intervals[interval]['label'], xaxe, yaxe, yaxe)

    if tabSelection == 'tabs2':
        if outputData is None or outputData == {}:
            raise PreventUpdate
        dff = pd.DataFrame(outputData)
        dff['Datetime'] = pd.to_datetime(dff['Datetime'])
        return linechart(dff, None, None, xaxe, yaxe, yaxe, "Plot " + xaxe + " against " + yaxe + " for [" + instrumentName + "]")
    if tabSelection == 'tabs3':
        if n_clicks is None or n_clicks == 0:
            raise PreventUpdate

        changed_id = [p['prop_id'] for p in callback_context.triggered][0]
        if alphas is None or alphas == {}:
            raise PreventUpdate
        if 'alphaselectionbutton' in changed_id:
            return PlotEstimateTarget(alphas, trainingPerc, settings, optim, outputData)
    return {}


@app.callback(
    Output("alphadropdown", "options"),
    Input("storageOutput", "data"),
    prevent_initiall_call=True,
)
def setAlphaSelections(inputData):
    if not (inputData is None or inputData == {}):
        dff = pd.DataFrame(inputData)
        options = [
            {
                "label": x, "value": x}
            for x in sorted(dff.columns) if not str(x).startswith('Datetime') and
                                            not str(x).startswith('CLOSE') and
                                            not str(x).startswith('CLOSETMR') and
                                            not str(x).startswith('OPENTMR')
        ]
        return options

    return None


if __name__ == "__main__":
    app.run_server(port=888, host="localhost", debug=True)
