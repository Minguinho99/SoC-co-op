<?php

    $con = mysqli_connect('localhost', 'root', 'root', 'chatting');

    //check connection happend
    if (mysqli_connect_errno()) {
        echo "1: Connection failed";//error #1 connection failed
        exit();
    }

    $username = $_POST["name"];
    $nickname = $_POST["nickname"];

    $namecheckquery = "Select username FROM user_info WHERE username='" . $username . "';";

    $namecheck = mysqli_query($con, $namecheckquery) or die("2: Name check query failed");
    if (mysqli_num_rows($namecheck) != 1) {
        echo "5: Either no user with name, or more than one"; //error #3 name exists cannot register
        exit();
    }

    $updatequery = "UPDATE user_info SET nickname = " . $nickname . " WHERE username = '" . $username . "';";
    mysqli_query($con, $updatequery) or die("7: Save query failed"); // error code #7

    echo "0";

?>