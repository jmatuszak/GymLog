
//Find exercise
$(document).ready(function () {
    var $table = $('#exerciseTable');
    var $rows = $table.find('tbody tr');

    $('#filterInput').on('input', function () {
        var filterValue = $(this).val().toLowerCase();

        $rows.each(function () {
            var name = $(this).find('td:nth-child(2)').text().toLowerCase();
            var shouldShow = filterValue === "" || name.includes(filterValue);

            $(this).toggle(shouldShow);
        });

        $table.css('display', 'block');
    });
});
//Modal add exercise
document.addEventListener("DOMContentLoaded", function () {
    var openModalBtn = document.getElementById("openModalBtn");
    var modal = document.getElementById("myModal");
    var closeBtn = document.getElementsByClassName("close")[0];

    openModalBtn.addEventListener("click", function (event) {
        modal.style.display = "block";
    });

    closeBtn.addEventListener("click", function () {
        modal.style.display = "none";
    });
});


//Partial render best 

$(document).ready(function () {
    $('#maxWeightButton').click(function () {
        var id = $(this).data('id');

        $.ajax({
            url: '@Url.Action("PartialStatistic", "Workout")',
            type: 'GET',
            data: { id: id },
            success: function (result) {
                $('#partialViewContainer').html(result);
            },
            error: function (xhr, status, error) {
                console.log(error);
            }
        });
    });
});