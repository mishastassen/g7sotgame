/* global __dirname */
var express = require('express');
var session = require('express-session');
var bodyParser = require('body-parser');
var cookieParser = require('cookie-parser');
var mysql = require('mysql');

/*Connect to MySQL database */
var connection = mysql.createConnection({
  host     : 'localhost',
  user     : 'ewi3620tu7',
  password : 'ayRoHef3',
  database : 'ewi3620tu7'
});
connection.connect();

/*select port for server*/
var port = 8088;

/*Initializng express */
var app = express();
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
  extended: true
}));
app.use(cookieParser());
app.use( session({cookieName: 'session', 
				  secret: 'Geheim',
				  resave: true,
				  saveUninitialized: true,
				  cookie: {}}));

/*Session variable*/
var sess;

/*Different express urls*/
app.get('/',function(req,res){
	console.log('Empty url received');
	sess = req.session;
	if(sess.loggedin === true){
		res.redirect('/response');
	}
	else{
		res.end('Login first');
	}
});

app.post('/createAccount',function(req,res){
	console.log('User trying to create account');
	sess = req.session;
	if(sess.loggedin === true){
		res.end("Log out first");
	}
	else{
		var username = req.body.username,
			pass = req.body.password;
		console.log("Creating new account " + username +" " + pass);
		res.end("Account created");
	}
});

app.post('/login',function(req,res){
	console.log('User loggin in');
	sess = req.session;
	var username = req.body.username,
		pass = req.body.password;
	if (username === 'admin' && pass === 'root'){
		sess.username = "admin";
		sess.loggedin = true;
		console.log('Password correct');
		res.end("you logged in!");
	}
	else{
		console.log('Wrong Password');
		res.end("Wrong username or password");
	}
});


app.get('/response',function(req,res){
	console.log('User asking for response');
	sess = req.session;
	if(sess.loggedin === true){
		res.end('Logged in as ' + sess.username);
	}else{
		res.end('Not logged in');
	}
});


app.get('/logout',function(req,res){
	req.session.destroy(function(err){
		if(err){
			console.log(err);
		}
		else{
			console.log('User logged out');
			res.end("You logged out");
		}
	});
});

/*Start server*/
var server = app.listen(port, function () {
  var port = server.address().port;

  console.log('Server listening at %s', port);
});
