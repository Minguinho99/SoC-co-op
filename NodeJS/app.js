//mysql
var usersRouter = require('./routes/users');
var sequelize = require('./test/models/index').sequelize;

var app = express();
sequelize.sync();

//socket
var io = require('socket.io')(process.env.PORT || 8080)

console.log('Server has started');

io.on('connection', function(socket) {
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