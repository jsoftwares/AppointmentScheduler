$(document).ready(function () {
    intitializeCalendar();
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

const onCloseModal = () => $('#appointmentInput').modal('hide');