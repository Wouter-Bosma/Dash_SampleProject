import dash_bootstrap_components as dbc
from documentation import *
from SecondLevelLayout import *

description = dbc.Card(
    [
        dbc.CardHeader("Description of objectives"),
        dbc.CardBody(description_text),
    ]
)

retrospect = dbc.Card(
    [
        dbc.CardHeader("Reflect on the concept"),
        dbc.CardBody(retrospective_text),
    ]
)

main = dbc.Row(
    [
        dbc.Col(secondaryTabs, width=12, lg=5, className="mt-4 border"),
        dbc.Col(
            [
                dbc.Row(id="mainGraphs"),
                dbc.Row(id="axeSelections")
            ]
        )
    ]
)

primaryTabs = dbc.Tabs(
    [
        dbc.Tab(description, tab_id="tabp1", label="Description"),
        dbc.Tab(main, tab_id="tabp2", label="MainTab"),
        dbc.Tab(retrospect, tab_id="tabp3", label="Retrospect"),
    ]
)

