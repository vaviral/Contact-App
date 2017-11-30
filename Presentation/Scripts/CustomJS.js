function SendMessageFunc(FName,LName,Contact) {
    window.location = "/Account/Compose?FName=" + FName + "&LName=" + LName + "&Contact=" + Contact;
}
function Contacts() {
    document.getElementById("Menu").value = "Contacts";
}
function SentMessages() {
    document.getElementById("Menu").value = "SentMessages";
}
function AddContact() {
    document.getElementById("Menu").value = "AddContact";
}
function Login() {
    document.getElementById("Menu").value = "Login";
}
function Logout() {
    document.getElementById("Menu").value = "Logout";
}
$(document).ready(function () {
    var value = '#' + document.getElementById("Menu").value;
    $(value).addClass('active');
});