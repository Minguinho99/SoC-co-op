// var io = require('socket.io').listen(8080)
//mysql
var express    =  require("express");
var mysql      = require('mysql');
const bodyParser = require('body-parser');

var conn = mysql.createConnection({
    host     : 'localhost',
    port     : 3306,
    user     : 'root',
    password : 'root',
    database : 'chatting',
    socketPath : '/Applications/MAMP/tmp/mysql/mysql.sock'
});

var app = express();
app.use(bodyParser.urlencoded({ extended: true }));//post body 사용가능하게
app.use(bodyParser.json());

conn.connect(function(err){
    if(!err) {
      console.log("Database is connected ... \n\n");
    } else {
      console.log("Error connecting database ... \n\n");
    }
});

app.post("/signin",function(req,res){
    var username = req.body.username;
    var nickname = req.body.nickname;
    var password = req.body.password;

    conn.query('SELECT username FROM user_info WHERE username = ?', username, function(err, rows, fields) {
        if (err) {
            res.status(500).send('Internal Server Error');
            console.log(err);
            console.log('Error while performing Query.');
        }
        else {
            var result_json = {};
            if (rows[0] == undefined) {
                conn.query('INSERT INTO user_info (nickname, username, password) values(?, ?, ?)', [nickname, username, password], function(err, rows, fields) {
                    if (err) {
                        res.status(500).send('Internal Server Error');
                        console.log(err);
                        console.log('Error while performing Query.');
                    }
                    else {
                        result_json["state"] = 'Success';
                        result_json["userid"] = username;
                        res.json(result_json);
                    }
                })
            }
            else {
                result_json["state"] = 'there are exsit user';
                result_json["userid"] = username;
                res.json(result_json);
            }
        }
    })
})

app.post('/login', function(req, res) {
    var username = req.body.name;
    var password = req.body.password;
    
    conn.query('SELECT username FROM user_info WHERE username = ?', username, function(err, rows, fields) {
        if (err) {
            res.send("error");
            console.log("error is: " + err);
            console.log('Error while performing Query');
        }
        else {
            if (rows[0] == undefined) {
                console.log('There is no such id');
                var result_json = {};
                result_json["state"] = 'There is no such ID';
                result_json["success"] = false;
                res.json(result_json);
            }
            else {
                conn.query('SELECT password, nickname FROM user_info WHERE username = ?', username, function(err, rows, fields) {
                    if (err) {
                        res.send("error");
                        console.log("error is : " + err);
                        console.log('Error while performing Query');
                    }
                    else {
                        if (rows[0].password == password) {
                            var result_json = {};
                            result_json["state"] = 'Success';
                            result_json["success"] = true;
                            res.json(result_json);
                            console.log('loginSuccess');
                        }
                        else {
                            var result_json = {};
                            result_json["state"] = 'Password is wrong';
                            result_json["success"] = false;
                            res.json(result_json);
                            console.log('right password : ' + rows[0].password);
                        }
                    }
                })
            }
        }
    })
});
    
app.post('/password', function(req, res) {
    var nickname = req.body.nickname;
    var username = req.body.username;
    
    conn.query('SELECT password, nickname FROM user_info WHERE username = ?', username, function(err, rows, fields) {
        if (err) {
            res.send("error");
            console.log("error is: " + err);
            console.log('Error while performing Query');
        }
        else {
            if (rows[0] != undefined) {
                if (rows[0].nickname == nickname) {
                    console.log('Success Find Password');
                    var result_json = {};
                    result_json["state"] = 'Success';
                    result_json["password"] = rows[0].password;
                    result_json["success"] = true;
                    res.json(result_json);
                }
                else {
                    var result_json = {};
                    result_json["state"] = 'Fail';
                    result_json["password"] = '';
                    result_json["success"] = false;
                    res.json(result_json);
                }
            }
            else {
                var result_json = {};
                result_json["state"] = 'Fail';
                result_json["password"] = '';
                result_json["success"] = false;
                res.json(result_json);
            }
        }
    })
});

app.listen(3000, function(){
    console.log('Connected 3000 port!');
});

//socket

var io = require('socket.io')(process.env.PORT || 8080)

console.log('Server has started');

io.on('connection', function(socket) {

    console.log('Connection Made!');
    socket.emit('new', {hello: 'world'});

    socket.on('userName', function(data) {
        console.log('new username : ' + data);
        socket.emit('newUser', data);
    });
    
    socket.on('message', function(data) {
        const splitText = data.split('|');
        console.log(splitText[0] + ' : ' + splitText[1]);
        socket.emit('Rmessage', data);
    })

    socket.on('disconnect', function(data) {
        console.log('A player has disconnected');
        socket.emit('disUser', data);
    });
});