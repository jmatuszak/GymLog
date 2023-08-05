
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

    const closeModal = document.querySelector('.close');
    closeModal.addEventListener('click', function () {
        modal.style.display = 'none';
    });

    window.addEventListener('click', function (event) {
        if (event.target === modal) {
            modal.style.display = 'none';
        }
    });
});

//Modal animation view
document.addEventListener('DOMContentLoaded', function () {
    const openModalButtons = document.querySelectorAll('.openAnimationModalButton');
    const modal = document.getElementById('animationModal');
    const modalImage = document.getElementById('modalImage');

    openModalButtons.forEach(button => {
        button.addEventListener('click', function () {
            const itemId = button.getAttribute('data-id');

            const imagePath = '/images/' + itemId + '.webp';
            modalImage.src = imagePath; // Ustawienie src obrazu

            modal.style.display = 'block';
        });
    });

    const closeModal = document.querySelector('.close');
    closeModal.addEventListener('click', function () {
        modal.style.display = 'none';
    });

    window.addEventListener('click', function (event) {
        if (event.target === modal) {
            modal.style.display = 'none';
        }
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



