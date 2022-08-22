import dash_bootstrap_components as dbc
from DataInputLayout import baseDataInput
from DomainSpecificLanguageLayout import dslInput
from PlotAnalysis import plotAnalysis

secondaryTabs = dbc.Tabs(
    [
        dbc.Tab(baseDataInput, tab_id="tabs1", label="Data Input"),
        dbc.Tab(dslInput, tab_id="tabs2", label="Domain Language Input"),
        dbc.Tab(plotAnalysis, tab_id="tabs3", label="Data analysis"),
    ],
    id="secondaryTabsSelection",
)
