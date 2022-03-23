const Helpers = require('./../helpers/helpers');

class SiteUserRepository {

    constructor(db) {
        this.db = db;
        console.log(`${Helpers.getDateNowString()} HELLO from SiteUserRepository constructor`);
    }

    async findAll() {
        return new Promise(resolve => {
            var sql = 'SELECT Id, Username, Email, Password, CreatedAt, UpdatedAt FROM SiteUser';
            var params = [];
            this.db.all(sql, params, (err, rows) => {
                if (err) {
                    return console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                }

                console.log(`${Helpers.getDateNowString()} findAll returns ${rows.length} rows`);
                resolve(rows);
            });
        });
    }

    getAllUsers(res) {
        var sql = 'SELECT Id, Username, Email, Password, CreatedAt, UpdatedAt FROM SiteUser';
        var params = [];
        this.db.all(sql, params, (err, rows) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                res.status(400).json({ "error": err.message });
                return;
            }
            res.json({
                "message": "success",
                "data": rows
            });
            console.log(`${Helpers.getDateNowString()} getAllUsers returns ${rows.length} rows`);
        });
    }

    async findById(id) {
        return new Promise(resolve => {
            var sql = 'SELECT Id, Username, Email, Password, CreatedAt, UpdatedAt FROM SiteUser WHERE Id = ?'
            var params = [id];
            this.db.get(sql, params, (err, row) => {
                if (err) {
                    return console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                }

                if (row != undefined) {
                    console.log(`${Helpers.getDateNowString()} findById returns Id ${row.Id}`);
                }

                resolve(row);
            });
        });
    }

    getUserById(res, id) {
        var sql = 'SELECT Id, Username, Email, Password, CreatedAt, UpdatedAt FROM SiteUser WHERE Id = ?'
        var params = [id];
        this.db.get(sql, params, (err, row) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                res.status(400).json({ "error": err.message });
                return;
            }
            res.json({
                "message": "success",
                "data": row
            });
            console.log(`${Helpers.getDateNowString()} getUserById returns Id ${row.Id}`);
        });
    }

    async findByUsername(username) {
        return new Promise(resolve => {
            var sql = 'SELECT Id, Username, Email, Password, CreatedAt, UpdatedAt FROM SiteUser WHERE Username = ?'
            var params = [username];
            this.db.get(sql, params, (err, row) => {
                if (err) {
                    return console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                }

                if (row != undefined) {
                    console.log(`${Helpers.getDateNowString()} findByUsername returns row with id: ${row.Id}`);
                }
                resolve(row);
            });
        });
    }

    getUserByUsername(res, username) {
        var sql = 'SELECT Id, Username, Email, Password, CreatedAt, UpdatedAt FROM SiteUser WHERE Username = ?'
        var params = [username];
        this.db.get(sql, params, (err, row) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                res.status(400).json({ "error": err.message });
                return;
            }
            res.json({
                "message": "success",
                "data": row
            });
            console.log(`${Helpers.getDateNowString()} getUserByUsername returns Id ${row.Id}`);
        });
    }

    async findByEmail(email) {
        return new Promise(resolve => {
            var sql = 'SELECT Id, Username, Email, Password, CreatedAt, UpdatedAt FROM SiteUser WHERE Email = ?'
            var params = [email];
            this.db.get(sql, params, (err, row) => {
                if (err) {
                    return console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                }

                if (row != undefined) {
                    console.log(`${Helpers.getDateNowString()} findByEmail returns ${JSON.stringify(row)}`);
                }

                resolve(row);
            });
        });
    }

    getUserByEmail(res, email) {
        var sql = 'SELECT Id, Username, Email, Password, CreatedAt, UpdatedAt FROM SiteUser WHERE Email = ?'
        var params = [email];
        this.db.get(sql, params, (err, row) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                res.status(400).json({ "error": err.message });
                return;
            }
            res.json({
                "message": "success",
                "data": row
            });
            console.log(`${Helpers.getDateNowString()} getUserByEmail returns Id ${row.Id}`);
        });
    }

    // Insert new user
    async create(username, email, password) {
        return new Promise(resolve => {
            var errors = [];

            if (!password) {
                errors.push("No Password is specified!");
            }

            if (errors.length) {
                return console.error(`${Helpers.getDateNowString()} ERROR: ${errors.join(", ")}`);
            }

            var data = {
                Username: username,
                Email: email,
                Password: password,
                CreatedAt: new Date()
            };

            var params = [data.Username, data.Email, data.Password, data.CreatedAt, data.CreatedAt];

            var sql = 'INSERT INTO User (Username, Email, Password, CreatedAt, UpdatedAt) VALUES (?, ?, ?, ?, ?)';
            this.db.serialize(() => {
                this.db.run(sql, params, (err) => {
                    if (err) {
                        return console.error(`${Helpers.getDateNowString()} INSERT ERROR: ${err.message}`);
                    }
                });
                this.db.get("SELECT Id, Username, Email, Password, CreatedAt, UpdatedAt FROM SiteUser WHERE Id = (SELECT seq FROM sqlite_sequence WHERE name = 'User')", [], (err, row) => {
                    if (err) {
                        return console.error(`${Helpers.getDateNowString()} GET ERROR: ${err.message}`);
                    }

                    resolve(row);
                });
            });
        });
    }

    insertNewUser(res, username, email, password) {
        var errors = [];

        if (!created) {
            errors.push("No Created is specified!");
        }

        if (errors.length) {
            console.error(`${Helpers.getDateNowString()} ERROR: ${errors.join(", ")}`);
            res.status(400).json({ "error": errors.join(", ") });
            return;
        }

        var data = {
            Username: username,
            Email: email,
            Password: password,
            CreatedAt: new Date().getUTCDate()
        };

        var params = [data.Username, data.Email, data.Password, data.CreatedAt, data.CreatedAt];

        var sql = 'INSERT INTO User (Username, Email, Password, CreatedAt, UpdatedAt) VALUES (?, ?, ?, ?, ?)';
        this.db.serialize(() => {
            this.db.run(sql, params, (err) => {
                if (err) {
                    console.error(`${Helpers.getDateNowString()} INSERT ERROR: ${err.message}`);
                    res.status(400).json({ "error": err.message });
                    return;
                }
            });
            this.db.get("SELECT Id FROM SiteUser WHERE Id = (SELECT seq FROM sqlite_sequence WHERE name = 'User')", [], (err, row) => {
                if (err) {
                    console.error(`${Helpers.getDateNowString()} GET ERROR: ${err.message}`);
                    res.status(400).json({ "error": err.message });
                    return;
                }

                console.log(`${Helpers.getDateNowString()} insertNewUser inserted row: ${JSON.stringify(row)}`);
                res.json({
                    "message": "success",
                    "data": row
                });
            });
        });
    }

    // Update User
    updateUserById(res, id, username, email, password) {
        var data = {
            Id: id,
            Username: username,
            Email: email,
            Password: password,
            UpdatedAt: new Date().getUTCDate()
        };

        var sql = 'UPDATE User SET Username = COALESCE(?, Username), Email = COALESCE(?, Email), Password = COALESCE(?, Password), UpdatedAt = ? WHERE Id = ?'
        var params = [data.Username, data.Email, data.Password, data.UpdatedAt, data.Id];
        this.db.run(sql, params, (err, result) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                res.status(400).json({ "error": res.message })
                return;
            }
            res.json({
                message: "success",
                data: data,
                changes: this.changes
            });
            console.log(`${Helpers.getDateNowString()} updateUserById: Id: ${data.Id} => Update: ${JSON.stringify(data)}`);
        });
    }

    // Delete User
    deleteUser(res, id) {
        var sql = 'DELETE FROM SiteUser WHERE Id = ?';
        var params = [id];
        this.db.run(sql, params, (err, result) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                res.status(400).json({ "error": res.message })
                return;
            }
            res.json({
                "message": "deleted",
                "id": id
            });
            console.log(`${Helpers.getDateNowString()} deleteUserId: ${id}`);
        });
    }
}

module.exports = SiteUserRepository;