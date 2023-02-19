var url = "https://conscount02.azurewebsites.net/api/ConsCount02/";

var xhr = new XMLHttpRequest();
xhr.open("POST", url);

xhr.setRequestHeader("Content-Type", "application/json");

xhr.onreadystatechange = function () {
if (xhr.readyState === 4) {
    console.log(xhr.status);
    console.log(xhr.responseText);
    document.getElementById("counter").innerHTML = xhr.responseText;
}};

var data = '{ "URL": "TEST" } ';

xhr.send(data);