$(function () {
    var sortMsg = "</br>  <small class='green'>(click <i class='fa fa-hand-pointer-o' aria-hidden='true'></i> to sort)</small>";
    var messages = {
        textAreInput: "Input in Text Area",
        emptyInputs: "No input or the input is not in English. Plese enter text in English or url to Engish page",
        emptyUrlInput: "External source is empty",
        emptyExternalSourceResponse: "External source service has problems ",
        noData:"No data is retrieved",
        serverError:"Oops Something went wrong"
    };

    function intializeSortableTable(containerId, data, numberOfColums, colsTitles) {
        var tableFields = [];
        switch (numberOfColums) {
            case 1:
                tableFields = [{ name: "Key", type: "text", width: "100%", title: colsTitles[0] }];
                break;
            case 2:
                tableFields = [
                { name: "Key", type: "text", width: "50%", title: colsTitles[0] },
                { name: "Value", type: "number", width: "50%", title: colsTitles[1] }
                ]
                break;
        }

        $("#" + containerId).jsGrid({
            //height: "100%", //fix for scrollbar issue
            width: "100%",
            sorting: true,
            paging: true,
            data: data,
            fields: tableFields
        });
    }

    function AddBootsrapGridClasses() {
        var tablesCssClass = "col-sm-4 col-md-4 col-lg-4";
        var numberOfTables = $('.jsgrid-grid-header').length;
        console.log(numberOfTables);
        switch (numberOfTables) {
            case 1: 
                tablesCssClass = "col-sm-12 col-md-12 col-lg-12";
                break;
            case 2:
                tablesCssClass = "col-sm-6 col-md-6 col-lg-6";
                break;
        }
        
        $("#jsGrid-analyzer-results-keywords").parent().removeClass();
        $("#jsGrid-analyzer-results-keywords-meta").parent().removeClass();
        $("#jsGrid-analyzer-results-external-links").parent().removeClass();

        $("#jsGrid-analyzer-results-keywords").parent().addClass(tablesCssClass);
        $("#jsGrid-analyzer-results-keywords-meta").parent().addClass(tablesCssClass);
        $("#jsGrid-analyzer-results-external-links").parent().addClass(tablesCssClass);
    }

    function addTitle(jsonData) {
        var analisysTitle = $('#analisys-object-title');
        if (jsonData.ExternalSourceUrl) {
            analisysTitle.find('span').html(jsonData.ExternalSourceUrl);
        }
        else if($('#internal-source').val()){
            $('#analisys-object-title').find('span').html(messages.textAreInput);
        }
        else {
            $('#analisys-object-title').find('span').html(messages.emptyInputs);
        }

        $('#analisys-object-title').removeClass('hidden');
    }

    function addEmptyResultErrorMsg(jsonData) {
        console.log(jsonData);
        if (jsonData.ExternalSourceUrl && jsonData.ResponseStatus == "Success") {
            $('#error-msg').html("External source is empty");
        }
        else if (jsonData.ExternalSourceUrl && jsonData.ResponseStatus == "Failed") {
            $('#error-msg').html(messages.emptyExternalSourceResponse + jsonData.ResponseStatusMsg);
        }
        else if (jsonData.ExternalSourceUrl && jsonData.ResponseStatus == "NotResolved") {
            $('#error-msg').html(messages.emptyExternalSourceResponse + jsonData.ResponseStatusMsg);
        }
        else {
            $('#error-msg').html(messages.emptyInputs);
        }

        $('#error-msg').removeClass('hidden');
    }

    function clearResult() {
        $('.table-results-container').empty();
        $('#error-msg').empty().addClass('hidden');
        $('#external-source').val('');
        $('#analisys-object-title').addClass('hidden');
    }

    $('#button-text-analyzer').click(function (ev) {
        ev.preventDefault();
        clearResult();
        $('#analyze-else').addClass('hidden');
        $("#submit-button").removeClass('hidden');
        $("#text-box-input-container").removeClass('hidden');
        $("#url-input-container").addClass('hidden');
        $('#analyzer-filter-form').trigger("reset");
        $('html, body').animate({
            scrollTop: $("#text-box-input-container").offset().top
        }, 2000);

        return false;
    });

    $('#button-url-analyzer').click(function (ev) {
        ev.preventDefault();
        clearResult();
        $('#analyze-else').addClass('hidden');
        $("#submit-button").removeClass('hidden');
        $("#url-input-container").removeClass('hidden');
        $("#text-box-input-container").addClass('hidden');
        $('#analyzer-filter-form').trigger("reset");
        $('html, body').animate({
            scrollTop: $("#url-input-container").offset().top
        }, 2000);

        return false;
    });

    $('#analyze-else').click(function (ev) {
        clearResult();
        $('#analyzer-filter-form').trigger("reset");
        $("html, body").animate({ scrollTop: 0 }, "slow");
        $('#analyze-else').addClass('hidden');
        return false;
    });

    $("#analyzer-filter-form").on("submit", function (event) {
        event.preventDefault();
        var form = $(this);
        var collection = form.serialize();
        var url = form.attr("action");
        $.ajax({
            url: url,
            data: collection,
            type: 'POST',
            xhr: function () { // progressbar
                var xhr = $.ajaxSettings.xhr();
                if (xhr.upload) {
                    xhr.upload.addEventListener('progress', function () {
                        $('#analyze-else').addClass('hidden');
                        $('#preloader').removeClass("hidden");
                        clearResult();
                    }, false);
                }
                return xhr;
            },
            success: function (data) {
                $('#preloader').addClass("hidden");
                $('html, body').animate({
                    scrollTop: $("#analisys-results-container").offset().top
                }, 2000);
                if (data) {
                    var jsonData = JSON.parse(data);
                    addTitle(jsonData);
                    if (jsonData.WordsFrequencies && jsonData.WordsFrequencies.length > 0) {
                        var titles = ["Keyword" + sortMsg, "Frequency" + sortMsg];
                        intializeSortableTable("jsGrid-analyzer-results-keywords", jsonData.WordsFrequencies, 2, titles);
                        
                    }
                    else {
                        addEmptyResultErrorMsg(jsonData); 
                    }

                    if (jsonData.MetaKeywordsFrequencies && jsonData.MetaKeywordsFrequencies.length > 0) {
                        var titles = ["Keyword in Meta" + sortMsg, "Frequency" + sortMsg];
                        intializeSortableTable("jsGrid-analyzer-results-keywords-meta", jsonData.MetaKeywordsFrequencies, 2, titles);
                    }

                    if (jsonData.ExternalLinks && jsonData.ExternalLinks.length > 0) {
                        var titles = ["External Links" + sortMsg];
                        intializeSortableTable("jsGrid-analyzer-results-external-links", jsonData.ExternalLinks, 1, titles);
                    }

                    AddBootsrapGridClasses();
                    $('#analyze-else').removeClass('hidden');
                }
                else {
                    $('#error-msg').html(messages.noData);
                    $('#error-msg').removeClass('hidden');
                    $('#analyze-else').removeClass('hidden');
                }
            },
            error: function (err) {
                $('#preloader').addClass("hidden");
                $('#error-msg').html(messages.serverError);
                $('#error-msg').removeClass('hidden');
                $('#analyze-else').removeClass('hidden');
            }
        });

        return false;
    });
});