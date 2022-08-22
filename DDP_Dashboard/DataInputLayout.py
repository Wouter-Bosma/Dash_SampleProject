from dash import Dash, dcc, html, dash_table, Input, Output, State, callback_context
import dash_bootstrap_components as dbc
from documentation import instrumentSelectionText
from InputDataSelection import DowConstituents
from InputDataSelection import Intervals

intervalSelection = dbc.Card(
    [
        html.H4(
            "Select an interval for the simulation:",
            className="card-title",
        ),
        dbc.RadioItems(
            id="interval",
            options=[
                {"label": interval["label"], "value": i}
                for i, interval in enumerate(Intervals)
            ],
            value=2,
            labelClassName='mb-2'
        )
    ]
)

baseDataInput = dbc.Col(
    [
        instrumentSelectionText,
        dcc.Dropdown(DowConstituents, 'AAPL', id="baseinstrument-selection"),
        intervalSelection,
        dbc.Button(
            id='dataselectionbutton',
            children="Submit",
            n_clicks=0,
            color="primary",
            className="mt-3",
        ),
    ]
)