<?php

    $con = mysqli_connect('localhost', 'root', 'root', 'chatting');

    //check connection happend
    if (mysqli_connect_errno()) {
        echo "1: Connection failed";//error #1 connection failed
        exit();
    }

    $username = $_POST["name"];
    $passwordEnter = $_POST["password"];

    //check if name exists
    $namecheckquery = "Select username FROM user_info WHERE username='" . $username . "';";

    $namecheck = mysqli_query($con, $namecheckquery) or die("2: Name check query failed"); //error #2 name check query failed

    if (mysqli_num_rows($namecheck) != 1) {
        echo "5: Either no user with name, or more than one"; //error #3 name exists cannot register
        exit();
    }

    //get login info from query
    // $existinginfo = mysqli_fetch_assoc($namecheck);
    // $password = $existinginfo["password"];
    
    // $loginhash = crypt($passwordEnter, $password);
    $getpasswordquery = "Select password FROM user_info WHERE username='" . $username . "';";
    
    $password = mysqli_query($con, $getpasswordquery);
    $existinginfo = mysqli_fetch_assoc($password);
    $psw = $existinginfo["password"];
    if ($psw != $passwordEnter) {
        echo "6: Incorrect password";
        exit();
    }

    echo "0\t" . $existinginfo["nickname"];

?>
