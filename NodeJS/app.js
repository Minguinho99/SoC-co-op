// var io = require('socket.io').listen(8080)
//mysql
var express = require("express");

var mysql = require('mysql');
const bodyParser = require('body-parser');

var conn = mysql.createConnection({
    host    : 'localhost',
    port    : 3306,
    user    : 'root',
    password    : 'root',
    database    : 'chatting',
    socketPath  : '/Applications/MAMP/tmp/mysql/mysql.sock'
});

var app = express();
app.use(bodyParser.urlencoded({ extended: true}));
app.use(bodyParser.json());

conn.connect(function(err) {
    if (err) {
        console.log(err);
    }
    else {
        console.log('fine');
    }
});

// conn.query('SELECT password FROM user_info WHERE username = ?', 'dksl', function(err, rows, fields) {
//     if (err) {
//         res.send("error");
//         console.log("error is: " + err);
//         console.log('Error while performing Query');
//     }
//     else {
//         if (rows[0] == undefined) {
//             console.log('undifiend happy');
//         }
//         else {
//             console.log(rows[0]);
//         }
//     }
// })

app.get("/signinCheck", function(req, res) {
    console.log('connect');
});

app.post("/s", function(req,res){
    var user_id = req.body.username;
    var user_name = req.body.nickname;
    var password = req.body.password;
    // var sql = 'SELECT username FROM user_info WHERE username = ?';
    var sql = 'INSERT INTO user_info (nickname, username, password) values(?, ?, ?)';
    
    conn.query(sql, [user_id, user_name, password], function(err, rows, fields) {
      if (err)
      {
        res.send("error");
        console.log("error is:"+err);
        console.log('Error while performing Query.');
      }
      else
      {
        //   if (rows[0] == undefined) {
        //       res.send('0');
        //       console.log('0');
        //   }
        //   else {
        //       res.send('1');
        //       console.log('1');
        //   }
        console.log(rows[0]);
        // var resultJson = {};
        // resultJson["userid"] = rows[0].user_id;

        // res.json(resultJson);
        res.send(rows[0]);
      }
    });
  });

//socket

var io = require('socket.io')(process.env.PORT || 8080)

console.log('Server has started');

io.on('connection', function(socket) {
    //Signin Page
    socket.emit('signin', 'hello');

    socket.on('idCheck', function(data) {
        const splitText = data.split('|');

        conn.query('SELECT username FROM user_info WHERE username = ?', splitText[1], function(err, rows, fields) {
            if (err) {
                res.send("error");
                console.log("error is: " + err);
                console.log('Error while performing Query');
            }
            else {
                if (rows[0] == undefined) {
                    console.log('undifiend happy');
                    conn.query('INSERT INTO user_info (nickname, username, password) values(?, ?, ?)', [splitText[0], splitText[1], splitText[2]], function(err, rows, fields) {
                        socket.emit('signinSuc', 'Success!');
                    })
                }
                else {
                    console.log(rows[0]);
                    socket.emit('idExist', 'Name already exists');
                }
            }
        })
    })

    //Chatting Page
    var name = ''

    console.log('Connection Made!');
    socket.emit('new', {hello: 'world'});

    socket.on('userName', function(data) {
        console.log(data);
        name = data;
        socket.emit('newUser', name);
    });
    
    socket.on('message', function(data) {
        console.log(name + " : " + data);
        socket.emit('Rmessage', name + "|" + data);
    })

    socket.on('disconnect', function() {
        console.log('A player has disconnected');
        socket.emit('disUser', name);
    });
});