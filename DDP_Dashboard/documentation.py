from dash import dcc

description_text = dcc.Markdown(
    """
    The objective of this dashboard is to function as a proof of concept to showcase how basic building blocks
    can be used to dynamically interact with data and how interoperability can be achieved.

    The key components for investigation are:
    - Download data from an external source for further processing;
    - Run an external component to process data using DSL;
        * The Domain Specific Language (DSL) is implemented in C# as a test to see how human readable instruction
          can be easily interpreted via code to run basic analysis. Extending a DSL is relatively straightforward.
    - Retrieve back the output to further process the data using python libraries
    - Display the data with plotly libraries

    It was chosen to use C# libraries because general accessibility to this library but in the current set-up any command line
    interface is feasible to incorporate.
    
    Technically the libraries can be implemented using different technologies or have a different breakdown,
    the key takeaway is the interoperability and the usage of different techniques. Given the nature of the proof of concepts
    some of the building blocks can be reused and others will inevitably be discarded or used in a different context for
    different usages.
    """
)

retrospective_text = dcc.Markdown(
    """
    All in all this project demonstrates that with minimal coding complexity, it is possible to create an interactive webpage
    that allows users to intuitively interact with data and build better insights into the data. Strong points are that
    the python code is in general easy accessible and thus readable by a wide audience and that web applications can easily
    be updated with minimal interference to the end user. 

    Looking back at the proof of concept, there are some points that need to be further worked out. Examples of items to
    be worked out further:
    - Code organization; as code is growing organically there will be inherently a continuous refactoring process.
    - Testing; currently the not all code is covered with unit tests, code metrics with for example jetbrains allow
               for easy to spot code coverage and especially untested code paths. Although for a proof of concept it
               should be sufficient to give starting points.
    - Targeting; the POC demonstrates varies concepts, the real-life world most likely will specialize these individual
               concepts in more detailed versions.
    - Outcome; as expected the relatively simple models deployed do not reach any useful correlation or output. The goal was to study the feasibility
    and the difficults of each of the components and to gain insights of the strong/weak points.
    - Code synergy; currently operations are managed via files and command line output, this can be further improved by
    using other technologies like in this case a rest API.
    """
)

disclaimer = dcc.Markdown(
    """
    [Data source:](http://yahoo.com/)
    All data is sourced from Yahoo. Above representations are solely made for demonstration purposes. 
    """
)

instrumentSelectionText = dcc.Markdown(
    """
> This screen gives a quick selection method for selecting instruments and the time period (sample period) of the data. The underlying code will automatically download the maximum and most recent period matching the relevant data.
>
> For convenience reasons in this example only the instruments in the Dow Jones are pre configured in the drop down menu.
"""
)

domainSpecificLanguageText = dcc.Markdown(
    """
> The Domain Specific Language allows for instructing the C# layer behind to accept in English the basic instructions for calculating Alpha parameters.
> In this example the language only understands OPEN, HIGH, LOW, CLOSE, DELAY, SMA on a single (static) datasource, the one loaded on the previous page.
>
> The syntax is <output name>=<input>
> SMA = Single moving average and takes the parameter to take the average of (OPEN/HIGH/LOW/CLOSE) and the number of time to take the average of
> DELAY = A single historical point with (OPEN/HIGH/LOW/CLOSE) with n steps back
> 
> The output file will only contain the date and the defined output names, together with the default CLOSE, CLOSETMR, OPENTMR
> In the following tab the simulation will try to build a model based on the output names and make a prediction of either the close or open of tomorrow based on the current close and Alpha's.  
"""
)

simulationInputText = dcc.Markdown(
"""
> This section gives a quick overview of how a basic simulation looks like. The main ingredients are the:
* Selection of the alphas to use as input;
* Selection of the duration in % of sample set
    - ie. 10% would mean that the first 10% of the samples are used to estimate for the remaining 90% of data
    - for simplicity only a simple two way split is used for training and running estimation
* Method of normalization to scale them towards the target to measure;
    - Scale between -1..1
    - Subtract mean and divide std. dev.
* Approximation method of Alphas to estimate the move of the instrument;
    - Multiple linear regression to estimate return based on selected alphas
    - Multiple linear regression to estimate next day estimator directly from close and alphas
* Simulated target.
    - Plot errors of each estimator
    - Sum gains and losses of the chosen direction
* Do note that the simulation (submit simulation) can take quite a bit of time.
"""
)