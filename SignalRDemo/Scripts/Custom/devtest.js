var devTest = {

    options: {
        devTestId: 0
    },

    getAll: function () {

        $.ajax({
            url: '/Home/GetAll',
            contentType: 'application/html ; charset:utf-8',
            type: 'GET',
            dataType: 'html',
            success: function (result) {
                $('#dataTable').html(result);
            }
        });
    },

    add: function () {
        devTest.options.devTestId = 0;
        $("#txtCampaignName,#txtDate,#txtClicks,#txtConversions,#txtImpressions,#txtAffiliateName").val("");
        $("#devTestList").hide();
        $("#devTestForm").show();
    },

    edit: function (id) {

        devTest.options.devTestId = id;

        $.ajax({
            url: '/Home/Edit',
            type: 'POST',
            data: { id: devTest.options.devTestId },
            success: function (response) {
                $("#devTestList").hide();
                $("#devTestForm").show();
                $('#txtCampaignName').val(response.CampaignName);
                $('#txtDate').val(response.Date);
                $('#txtClicks').val(response.Clicks);
                $('#txtConversions').val(response.Conversions);
                $('#txtImpressions').val(response.Impressions);
                $('#txtAffiliateName').val(response.AffiliateName);
            },
            error: function (x, y, z) {
                alert("Server error.");
            }
        });
    },

    delete: function (id) {

        var ans = confirm("Are you sure to delete?");

        if (ans) {
            $.ajax({
                url: '/Home/Delete',
                type: 'POST',
                data: { id: id },
                success: function (response) {
                    if (response) {
                        alert('Data deleted Successfully.');
                    }
                    else {
                        alert('Server error.');
                    }
                },
                error: function (x, y, z) {
                    alert("Server Error.");
                }
            });
        }

    },

    save: function () {

        var obj = {
            ID: devTest.options.devTestId,
            CampaignName: $('#txtCampaignName').val(),
            Date: $('#txtDate').val(),
            Clicks: $('#txtClicks').val(),
            Conversions: $('#txtConversions').val(),
            Impressions: $('#txtImpressions').val(),
            AffiliateName: $('#txtAffiliateName').val()
        };

        $.ajax({
            url: '/Home/Save',
            type: 'POST',
            data: JSON.stringify(obj),
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                if (response) {
                    alert('Data saved successfully.');
                    $("#devTestList").show();
                    $("#devTestForm").hide();
                }
                else {
                    alert('Server error.');
                }
            },
            error: function () {
                alert('Server error');
            }
        });

    },

    cancel: function () {
        $("#devTestList").show();
        $("#devTestForm").hide();
    },
}

$(function () {
    // Create a proxy to signalr hub on web server. It reference the hub.
    var notificationFromHub = $.connection.employeeHub;

    // Connect to signalr hub
    $.connection.hub.start().done(function () {
        devTest.getAll();
    });

    // Notify to client with the recent updates
    notificationFromHub.client.updatedClients = function () {
        devTest.getAll();
    };
});