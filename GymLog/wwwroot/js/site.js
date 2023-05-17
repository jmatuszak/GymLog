$(document).ready(function () {
    $("#myButton").click(function () {
        var url = "/Exercise/FindExerciseModal";

        $("#myModal").css("display", "block");

        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $(".modal-content").html(data);

                $(".close").click(function () {
                    $("#myModal").css("display", "none");
                });
            }
        });
    });
});

var modal = document.getElementById("myModal");
var closeBtn = document.getElementsByClassName("close")[0];

closeBtn.onclick = function () {
    modal.style.display = "none";
}

//nowe

$(function () {
    $('#searchExercise').on('input', function () {
        var search = $(this).val().toLowerCase();
        var $exercises = $('#exerciseList .exercise');

        $exercises.each(function () {
            var exerciseName = $(this).text().toLowerCase();

            if (exerciseName.includes(search)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});

$(function () {
    $('#exerciseSelectButton').click(function () {
        var selectedExerciseId = $('#exerciseList .exercise.selected').data('exercise-id');
        $('#ExerciseId').val(selectedExerciseId);

        $('#exerciseModal').modal('hide');
    });

    $('#exerciseList').on('click', '.exercise', function () {
        $('#exerciseList .exercise').removeClass('selected');
        $(this).addClass('selected');
    });
});