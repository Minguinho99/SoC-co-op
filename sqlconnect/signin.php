<?php

    $con = mysqli_connect('localhost', 'root', 'root', 'chatting');

    //check connection happend
    if (mysqli_connect_errno()) {
        echo "Connection failed";//error #1 connection failed
        exit();
    }

    $nickname = $_POST["nickname"];
    $username = $_POST["username"];
    $password = $_POST["password"];
    $passwordCheck = $_POST["passwordCheck"];

    if ($nickname == "") {
        echo "Please input your name field";
        exit();
    }
    else if ($username == "") {
        echo "Please input your id field";
        exit();
    }
    else if ($password == "") {
        echo "Please input your password field";
        exit();
    }

    if ($password != $passwordCheck) {
        echo "Password does not same";
        exit();
    }

    //check if name exists
    $namecheckquery = "Select username FROM user_info WHERE username='" . $username . "';";

    $namecheck = mysqli_query($con, $namecheckquery) or die("Name check query failed"); //error #2 name check query failed

    if (mysqli_num_rows($namecheck) > 0) {
        echo "Name already exists"; //error #3 name exists cannot register
        exit();
    }

    //add user to the table
    $insertuserquesry = "INSERT INTO user_info (nickname, username, password) VALUES ('" . $nickname . "', '" . $username . "', '" . $password . "');";
    mysqli_query($con, $insertuserquesry) or die("Insert user_info query failed"); // error #4 insert query failed


    echo("0");

    

?>
