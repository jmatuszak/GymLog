
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
//Modal add exercise / List of exercises
document.addEventListener("DOMContentLoaded", function () {
    var openExercisesModalBtn = document.getElementById("openExercisesModalBtn");
    var modal = document.getElementById("exercisesModal");
    var closeBtn = document.getElementsByClassName("close")[0];

    openExercisesModalBtn.addEventListener("click", function (event) {
        modal.style.display = "block";
    });

    closeBtn.addEventListener("click", function () {
        modal.style.display = "none";
    });
});


//Partial render MaxWeight of exercise

$(document).ready(function () {
    $('#maxWeightButton').click(function () {
        var id = $(this).data('id');

        $.ajax({
            url: '@Url.Action("MaxWeight", "Workout")',
            type: 'GET',
            data: { id: id },
            success: function (result) {
                $('#maxWeightViewContainer').html(result);
            },
            error: function (xhr, status, error) {
                console.log(error);
            }
        });
    });
});
