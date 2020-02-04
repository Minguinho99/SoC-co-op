<?php

    $con = mysqli_connect('localhost', 'root', 'root', 'chatting');

    if (mysqli_connect_errno()) {
        echo "0";
        exit();
    }

    $nickname = $_POST["nickname"];
    $username = $_POST["username"];

    $namecheckquery = "Select password FROM user_info WHERE username='" . $username . "';";

    $password = mysqli_query($con, $namecheckquery) or die("0");

    if (mysqli_num_rows($password) != 1) {
        echo "0";
        exit();
    }

    $existinginfo = mysqli_fetch_assoc($password);
    $psw = $existinginfo["password"];

    echo $psw;

?>