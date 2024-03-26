$('.back-button').on('click', function (e) {


    $.ajax({
        url: '/AdminStatus/tabs',

        success: function (response) {
            $('#nav-new').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});
$('#regions').change(function () {
    var regionId = $(this).find(":selected").attr('id'); // This will get the id of the selected region

    $.ajax({
        url: '/Case/GetPhysicianByRegionId', // Replace with your server script URL
        type: 'GET',
        data: { regionId: regionId },
        success: function (data) {
            var secondDropdown = $('#physician'); // Replace with your second dropdown selector
            secondDropdown.empty(); // Clear existing options
            secondDropdown.append($('<option>', {
                hidden: "hidden",
                value: "",
                text: "Select Physician"
            }))
            $.each(data, function (index, item) {
                secondDropdown.append($('<option>', {
                    value: item.id, // Replace with the actual value from your data
                    text: item.first + " " + item.last // Replace with the actual text from your data
                }));
            });
        }
    });
});
$('#assign_case').on('click', function (e) {
    var requestid = $(this).attr('value');
    var physician = $('#physician').val();
    var addon = $('#float').val();

    $.ajax({
        url: '/Case/AssignCase',
        type: 'POST',
        data: { Id: requestid, admin_notes: addon, physician: physician },
        success: function (response) {
            $('#nav-new').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});
$('.viewNotes').on('click', function (e) {
    var requestid = $(this).attr('value');
    $.ajax({
        url: '/Case/ViewNotes',
        type: 'GET',
        data: { id: requestid },
        success: function (response) {
            $('#nav-home').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});