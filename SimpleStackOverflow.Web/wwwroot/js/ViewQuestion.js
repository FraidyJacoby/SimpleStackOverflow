$(() => {

    const questionId = $(".question").data('question-id');

    const like = {
        QuestionId : questionId,
        UserId: $("#like-button").data('user-id')
    }

    setLikeButton();

    $("#like-button").on('click', function () {
        $("#like-button").prop('disabled', true);
        $.post("/home/like", { like })
    })

    function setLikeButton() {
        console.log(like);
        $.get(`/home/checkifliked?questionId=${questionId}`, function (liked) {
            if (liked) {
                $("#like-button").prop('disabled', true);
            }
        })
    }

    setInterval(() => {
        console.log(questionId);
        $.get(`/home/getlikes?questionId=${questionId}`, function (count) {
            $("#likes-count").text(count);
        })
    }, 1000);

})