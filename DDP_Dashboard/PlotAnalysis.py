import dash_bootstrap_components as dbc
from documentation import simulationInputText
from dash import Dash, dcc, html, dash_table, Input, Output, State, callback_context


plotAnalysis = dbc.Col(
    [
        simulationInputText,
        # Alpha selection
        html.Div(
            html.Label('Select alphas for estimation', style={'font-weight': 'bold', "text-align": "center"}),
        ),
        html.Div(
            dcc.Dropdown
                (
                id="alphadropdown",
                multi=True,
            ),
            className="three columns",
        ),
        # slider
        html.Div(
            html.Label('Set rolling window percentage',
                       style={'font-weight': 'bold', "text-align": "center", "margin-top": "4px"}),
        ),
        html.Div(
            dcc.Slider(
                id="trainingperc",
                marks={i: f"{i}%" for i in range(0, 101, 10)},
                min=10,
                max=90,
                step=5,
                value=30,
                included=False,
            ),
        ),
        dbc.Card(
            [
                html.Div(
                    html.Label('Select how to normalize the alphas',
                               style={'font-weight': 'bold', "text-align": "center", "margin-top": "4px"}),
                ),
                html.Div(
                    dcc.Checklist(
                        id="normalizationsettings",
                        options=[{'label': 'Subtract mean', 'value': 'subtract'},
                                 {'label': 'divide by std. dev.', 'value': 'divide'}],
                        labelStyle=dict(display='block'),
                    )
                ),
            ],
            style={"margin-top": "10px"},
        ),
        dbc.Card(
            [
                html.Div(
                    html.Label('Chose what to estimate with the alphas',
                               style={'font-weight': 'bold', "text-align": "center", "margin-top": "4px"}),
                ),
                html.Div(
                    dcc.RadioItems(
                        id='optimization',
                        options=[{'label': 'Estimate return from alphas', 'value': 'return'},
                                 {'label': 'Estimate difference of close directly', 'value': 'difference'}],
                        labelStyle=dict(display='block'),
                        value='return',
                    )
                ),
            ],
            style={"margin-top": "10px"},
        ),
        dbc.Button(
                id='alphaselectionbutton',
                children="Submit simulation",
                n_clicks=0,
                color="primary",
                className="mt-2",
        ),
    ]
)

