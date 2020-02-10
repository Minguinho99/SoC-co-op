const express = require('express');
const router = express.Router();
const mysql = require("mysql");

let client = mysql.createConnection({
    user: "root",
    password: "kaijeon",
    database: "chatting"
})

module.exports = router;

router.post('/signin', function(req, res) {
    var nickname = req.nickname;
    var id = req.id;
    var password = req.password;

    client.query("INSERT INTO user_info (name, id, password) VALUES (?, ?, ?)", [
        nickname, id, password
    ], function() {
        res.redirect("/signin");
    });
});