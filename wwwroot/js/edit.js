namespace BE.wwwroot.js
{
    public class edit
    {
        function updateOptionSelection() {
        var questions = document.querySelectorAll(".question");
        for (var i = 0; i < questions.length; i++) {
            var question = questions[i];
            var options = question.querySelectorAll(".checkbox input[type='checkbox']");
            var atLeastOneSelected = false;
            for (var j = 0; j < options.length; j++) {
                if (options[j].checked) {
                    atLeastOneSelected = true;
                    break;
                }
            }
            if (!atLeastOneSelected) {
                question.classList.add("error");
            } else {
                question.classList.remove("error");
            }
        }
    }

    document.addEventListener("DOMContentLoaded", function () {
        updateOptionSelection();
        document.querySelectorAll(".checkbox input[type='checkbox']").addEventListener("change", updateOptionSelection);
    });

    }
}
