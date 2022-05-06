var routeURL = location.protocol + "//" + location.host;

$(document).ready(function () {
    intitializeCalendar();

    $("#appointmentDate").kendoDateTimePicker({
        value: new Date(),
        dateInput: false
    });
});

let calendar;
const intitializeCalendar = () => {
    try {
        var calendarEl = document.getElementById('calendar');
        if (calendarEl) {
            calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                headerToolbar: {
                    left: 'prev,next,today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
                selectable: true,
                editable: false,
                select:  (event) => {
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
                },
                eventClick: info => {
                    getEventDetailsByAppintmentId(info.event);
                }
            });
            calendar.render();
        }
        
    }
    catch (e) {
        alert(e);
    }
}

const onShowModal = (obj, isEditEvent) => {
    /**During Edit we pass our data we fetch from API when an aAppointment in FullCalendar is clicked (select), but when a date
     * is click to add a new Appointment, we pass d 'event' from SELECT to onShowModal; this event object contains the date
     * that was clicked as a property name 'startStr'. We can use this to get the date that was clicked so we can display that
     * on our Create modal instead of displaying the current date. startStr gives date without time.
     */
    if (isEditEvent !== null) {
        $('#id').val(obj.id);
        $("#title").val(obj.title);
        $('#description').val(obj.description);
        $("#appointmentDate").val(obj.startDate);
        $('#duration').val(obj.duration);
        $('#patientId').val(obj.patientId);
        $('#doctorId').val(obj.doctorId);
        $('#lblPatientName').val(obj.patientName);
        $('#lblDoctortName').val(obj.doctorName);
        if (obj.isDoctorApproved) {
            $('#lblStatus').val('Approved');
        }
        else {
            $('#lblStatus').val('Pending');
        }
    }
    else {
        $("#appointmentDate").val(obj.startStr + ' ' + new moment().format("hh:mm:A"));
        $('#id').val(0);
    }
    $('#appointmentInput').modal('show');
}

const onCloseModal = () => {
    $('#appointmentForm')[0].reset();
    $('#id').val(0);
    $("#title").val('');
    $('#description').val('');
    $("#appointmentDate").val('');
    $('#duration').val('');
    $('#patientId').val('');

    $('#appointmentInput').modal('hide');
    $("#title").removeClass('error');
    $("#appointmentDate").removeClass('error');

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
                    calendar.refetchEvents();
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

const getEventDetailsByAppintmentId = info => {
    $.ajax({
        url: routeURL + '/api/Appointment/GetCalendarDataById/' + info.id,
        type: 'GET',
        dataType: "json",
        success: response => {
            if (response.status === 1 && response.dataenum !== undefined) {
                onShowModal(respose.dataenum, true);
            }
            successCallback(events);
        },
        error: xhr => {
            $.notify("Error", "error");
        }
    });
}

const onDoctorChange = () => calendar.refetchEvents(); //fired when we change doctor in dropdown