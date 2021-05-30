window.onscroll = function sc() {
    
    // scroll to show

}
var counter = 1;
setInterval(function () {
    document.getElementById('radio' + counter).checked = true;
    counter++;
    if (counter > 4) {
        counter = 1;
    }
}, 5000);


function srollright() {

    var slidex = document.getElementById("slideCourse");
    slidex.scrollLeft = 100;
    var z = slidex.scrollLeft;
    if (true) {
        z = z + 100;
        slidex.scrollLeft = z;
    }

}
function scrollleft() {

    var slidex = document.getElementById("slideCourse");
    slidex.scrollLeft = 100;
    var z = slidex.scrollLeft;
    if (true) {
        z = z - 100;
        slidex.scrollLeft = z;
    }

}




