flatpickr("#datePicker", {
    mode: "range"
});
var modal = document.getElementById("myModal");
var tableBody = document.getElementById("tableBody")
// Get the button that opens the modal
var btn = document.getElementById("myBtn");
var modalBtn = document.getElementById("modalSubmit");
// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks the button, open the modal 
btn.onclick = function () {
    modal.style.display = "block";
}
modalBtn.onclick = function () {
    var name = document.getElementById("name").value
    var date = document.getElementById("datePicker").value
    var startdate = date.split(" to ")[0]
    var enddate = date.split(" to ")[1]
    var data = {}
    data.StartDate = startdate
    data.EndDate = enddate
    data.Name = name
    $.ajax({
        url: '/Exam/AddNewScheduling',
        type: 'POST',
        data: {
            dto: data
        },
        dataType: 'json',
        success: function (data) {
            if (data.enumResult == 200) {
                modal.style.display = "none";
                alert("تم إضافة دورة امتحانية جديدة بنجاح");
                let tr = document.createElement('tr');
                let nametd = document.createElement('td')
                nametd.textContent = data.result.name;
                nametd.classList.add("name")
                tr.appendChild(nametd)
                let starttd = document.createElement('td')
                starttd.textContent = data.result.startDate
                tr.appendChild(starttd)
                let endtd = document.createElement('td')
                endtd.textContent = data.result.endDate
                tr.appendChild(endtd)
                // insert a new node after the last list item
                tableBody.appendChild(tr)
            }
            else
                alert(data.ErrorMessages +"حدث خطأ ما يرجى إعلام الدعم الفني بالخطأ التالي")
        },
        error: function (request, error) {
            alert(error+"حدث خطأ تقني يرجى اعلام المختصين بالخطأ التالي" );
        }
    });
}
// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}