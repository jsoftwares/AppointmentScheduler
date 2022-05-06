var routeURL = location.protocol + "//" + location.host;

$(document).ready(function () {
    intitializeCalendar();

    $("#appointmentDate").kendoDateTimePicker({
        value: new Date(),
        dateInput: false
    });
});

const intitializeCalendar = () => {
    try {
        var calendarEl = document.getElementById('calendar');
        if (calendarEl) {
            var calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                headerToolbar: {
                    left: 'prev,next,today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
                selectable: true,
                editable: false,
                select: function (event) {
                    onShowModal(event, null);
                },
                eventDisplay: 'block',
                events: (fetchInfo, successCallback, failureCallback) => {
                    $.ajax({
                        url: routeURL + '/api/Appointment/GetCalendarData?doctorId=' + $('#doctorId').val(),
                        type: 'GET',
                        dataType: "json",
                        success: response => {
                            const events = [];
                            if (response.status === 1) {
                                $.each(response.dataenum, (i, data) => {
                                    events.push(
                                        {
                                            title: data.title,
                                            description: data.description,
                                            start: data.startDate,
                                            end: data.EndDate,
                                            id: data.id,
                                            backgroundColor: data.isDoctorApproved ? '#28a745' : '#dc3545',
                                            borderColor: '#162466',
                                            textColor: 'white'
                                        }
                                    );
                                });
                                
                            }
                            successCallback(events);
                        },
                        error: xhr => {
                            $.notify("Error", "error");
                        }
                    });
                }
            });
            calendar.render();
        }
        
    }
    catch (e) {
        alert(e);
    }
}

const onShowModal = (obj, isEventDetail) => {
    $('#appointmentInput').modal('show');
}

const onCloseModal = () => {
    $('#appointmentInput').modal('hide');

}

const onSubmitForm = () => {

    if (checkValidation()) {
        const requestData = {
            Id: parseInt($('#id').val()),
            Title: $("#title").val(),
            Description: $('#description').val(),
            StartDate: $("#appointmentDate").val(),
            Duration: $('#duration').val(),
            PatientId: $('#patientId').val(),
            DoctorId: $('#doctorId').val()
        }

        $.ajax({
            url: routeURL + '/api/Appointment/SaveCalendarData',
            type: 'POST',
            data: requestData,
            success: response => {
                if (response.status === 1 || response.status === 2) {
                    $.notify(response.message, "success");
                    onCloseModal();
                }
                else {
                    console.log(response);
                    $.notify(response.message, "error");
                }
            },
            error: xhr => {
                $.notify("Error", "error");
            }
        });
    }
}
    

const checkValidation = () => {
    let isValid = true;

    if ($("#title").val() === undefined || $("#title").val() === "") {
        isValid = false;
        $("#title").addClass('error');
    }
    else {
        $("#title").removeClass('error');
    }

    if ($("#appointmentDate").val() === undefined || $("#appointmentDate").val() === "") {
        isValid = false;
        $("#appointmentDate").addClass('error');

    }
    else {
        $("#appointmentDate").removeClass('error');
    }

    return isValid;
}