$(function () {

    var selectedMovie;

    $("#btnApiSearch").click(function () {

        $('li[class="myResultsList"]').remove();

        var term = $("#title").val();

        $.ajax("http://api.themoviedb.org/3/search/movie?api_key=d1dd5a7fd77c933e088112709eb711e7&query=" + term,
                {
                    method: "GET",
                    headers: { 'Accept': 'application/json' },
                }).success(function (myData) {
                    console.log('lets see:', JSON.stringify(myData));

                    for (var i = 0; i < myData.results.length; i++) {
                        $('#resultTitles').append('<li class="myResultsList" id="' + i + '"><a href="#">' + myData.results[i].title + '-' + myData.results[i].release_date + '</a></li>');
                    };

                    $('.dropdown').removeAttr('hidden');

                    $('.myResultsList').click(function () {
                        var movieIndex = parseInt(this.id);
                        selectedMovie = myData.results[movieIndex];
                        //alert(selectedMovie.id);

                        $.ajax("http://api.themoviedb.org/3/movie/" + selectedMovie.id + "?api_key=d1dd5a7fd77c933e088112709eb711e7&append_to_response=credits,videos",
                            {
                                method: "GET",
                                headers: { 'Accept': 'application/json' },
                            }).success(function (myMovie) {
                                console.log('lets see', JSON.stringify(myMovie));
                                $('#TmdbMovieId').val(myMovie.id);
                                $('#Title').val(myMovie.title);
                                $('#OriginalTitle').val(myMovie.original_title);
                                $('#Runtime').val(myMovie.runtime);
                                $('#ReleaseDate').val(myMovie.release_date);
                                $('#Tagline').val(myMovie.tagline);
                                $('#Synopsis').val(myMovie.overview);
                                $('#Score').val(myMovie.vote_average);
                                $('#VoteCount').val(myMovie.vote_count);
                                $('#Popularity').val(myMovie.popularity);
                                $('#PosterUrl').val("http://image.tmdb.org/t/p/w500" + myMovie.poster_path);
                                $('#BackdropPath').val("http://image.tmdb.org/t/p/w1280" + myMovie.backdrop_path);
                                $('#TrailerUrl').val("https://www.youtube.com/embed/" + myMovie.videos.results[0].key);
                                for (var i = 0; i < myMovie.credits.crew.length; i++) {
                                    if (myMovie.credits.crew[i].job === "Director") {
                                        $('#Director').val(myMovie.credits.crew[i].name);
                                        break;
                                    }
                                };
                            });
                    });
                }).error(function () {
                    alert("No response, try again in a few seconds or fill in the fields manually.")
                });
    });
});