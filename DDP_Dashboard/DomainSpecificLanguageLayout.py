from dash import Dash, dcc, html, dash_table, Input, Output, State, callback_context
import dash_bootstrap_components as dbc
from documentation import domainSpecificLanguageText

dslInput = dbc.Col(
    [
        domainSpecificLanguageText,
        dcc.Textarea(
            id='domainLanguageInput',
            value="""ALPHA1=CLOSE - DELAY(CLOSE, 5)
ALPHA2=OPEN - DELAY(CLOSE, 5)
ALPHA3=(HIGH * LOW) ^ 0.5 - SMA(CLOSE, 5)
ALPHA4=CLOSE + ((CLOSE + OPEN + HIGH + LOW) / 4 - OPEN)""",
            style={'width': '100%', 'height': 150, },
            title="DSL Input text",
        ),
        dcc.Textarea(
            id='defaultLanguageInput',
            value="""CLOSE=CLOSE
CLOSETMR=DELAY(CLOSE, -1)
OPENTMR=DELAY(OPEN, -1)""",
            style={'width': '100%', 'height': 100, },
            title="DSL Input text",
            readOnly=True
        ),
        dbc.Button(
            id='domainlanguagebutton',
            children="Submit",
            n_clicks=0,
            color="primary",
            className="mt-2",
        ),
    ]
)