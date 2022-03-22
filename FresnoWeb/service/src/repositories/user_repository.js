const Helpers = require('./../helpers/helpers');

class UserRepository {
    constructor(db) {
        this.db = db;
        console.log(`${Helpers.getDateNowString()} HELLO from UserRepository constructor`);
    }

    // Get all users
    getAllUsers(res) {
        var sql = 'SELECT Id, FirstName, LastName, Email, Street, PostCode, PostCity, BirthDate, Height, Sex, MaxHr FROM "User"';
        var params = [];
        this.db.all(sql, params, (err, rows) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getAllUsers ERROR: ${err.message}`)
            }
            res.json({
                "message": "success",
                "data": rows
            });
            console.log(`${Helpers.getDateNowString()} getAllUsers returns ${rows.length} rows`);
        });
    };

    // Get user by id
    getUserById(res, id) {
        var sql = 'SELECT Id, FirstName, LastName, Email, Street, PostCode, PostCity, BirthDate, Height, Sex, MaxHr FROM "User" WHERE Id = ?';
        var params = [id];
        this.db.get(sql, params, (err, row) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getUserById ERROR: ${err.message}`);
                res.status(400).json({ "error": err.message });
                return;
            }
            res.json({
                "message": "success",
                "data": row
            });
            console.log(`${Helpers.getDateNowString()} getUserById returns Id ${row.Id}`);

        });
    };

    // Get user by steptest id
    getUserByStepTestId(res, id) {
        var sql = 'SELECT u.Id, u.FirstName, u.LastName, u.Email, u.Street, u.PostCode, u.PostCity, u.BirthDate, u.Height, u.Sex, u.MaxHr FROM "User" u JOIN StepTest st ON st.UserId = u.Id WHERE st.Id = ?';
        var params = [id];
        this.db.get(sql, params, (err, row) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getUserByStepTestId ERROR: ${err.message}`);
                res.status(400).json({ "error": err.message });
                return;
            }
            res.json({
                "message": "success",
                "data": row
            });
            console.log(`${Helpers.getDateNowString()} getUserByStepTestId returns Id ${row.Id}`);

        });
    };

    // New User
    postNewUser(res, firstname, lastname, email, street, postcode, postcity, birthdate, height, sex, maxhr) {
        var errors = [];

        if (!firstname) {
            errors.push('No firstname specified');
        }

        if (!lastname) {
            errors.push('No lastname specified');
        }

        if (errors.length) {
            console.error(`${Helpers.getDateNowString()} ERROR: ${errors.join(", ")}`);
            res.status(400).json({ "error": errors.join(", ") });
            return;
        }

        var data = { FirstName: firstname, LastName: lastname, Email: email, Street: street, PostCode: postcode, PostCity: postcity, Height: height, BirthDate: birthdate, Sex: sex, MaxHr: maxhr };

        var sql = 'INSERT INTO User (FirstName, LastName, Email, Street, PostCode, PostCity, BirthDate, Height, Sex, MaxHr) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)';
        var params = [data.FirstName, data.LastName, data.Email, data.Street, data.PostCode, data.PostCity, data.BirthDate, data.Height, data.Sex, data.MaxHr];

        this.db.serialize(() => {
            this.db.run(sql, params, (err) => {
                if (err) {
                    console.error(`${Helpers.getDateNowString()} postNewUser ERROR: ${err.message}`);
                    res.status(400).json({ "error": err.message });
                    return;
                }
            });

            this.db.get("SELECT Id, FirstName, LastName, Email, Street, PostCode, PostCity, BirthDate, Height, Sex, MaxHr FROM User WHERE Id = (SELECT seq FROM sqlite_sequence WHERE name = 'User')", [], (err, row) => {
                if (err) {
                    console.error(`${Helpers.getDateNowString()} postNewUser ERROR: ${err.message}`);
                    res.status(400).json({ "error": err.message })
                    return;
                }

                res.json({
                    "message": "success",
                    "data": row,
                    "id": row.Id
                });
            });
        });
    };

    // Update user
    updateUserById(res, id, firstname, lastname, email, street, postcode, postcity, birthdate, height, sex, maxhr) {
        var data = { Id: id, FirstName: firstname, LastName: lastname, Email: email, Street: street, PostCode: postcode, PostCity: postcity, Height: height, BirthDate: birthdate, Sex: sex, MaxHr: maxhr };

        var sql = 'UPDATE User SET FirstName = COALESCE(?, FirstName), LastName = COALESCE(?, LastName), Email = COALESCE(?, Email), Street = COALESCE(?, Street), PostCode = COALESCE(?, PostCode), PostCity = COALESCE(?, PostCity), BirthDate = COALESCE(?, BirthDate), Height = COALESCE(?, Height), Sex = COALESCE(?, Sex), MaxHr = COALESCE(?, MaxHr) WHERE Id = ?';
        var params = [data.FirstName, data.LastName, data.Email, data.Street, data.PostCode, data.PostCity, data.BirthDate, data.Height, data.Sex, data.MaxHr, data.Id];
        this.db.run(sql, params, (err, result) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} updateUserById ERROR: ${err.message}`);
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
    };

    // Delete User
    deleteUserById(res, id) {
        var sql = 'DELETE FROM User WHERE Id = ?';
        var params = [id];
        this.db.run(sql, params, (err, result) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} deleteUser ERROR: ${err.message}`);
                res.status(400).json({ "error": res.message })
                return;
            }
            res.json({
                "message": "deleted",
                "id": id
            });
            console.log(`${Helpers.getDateNowString()} deleteUser: ${id}`);
        });
    };
}

module.exports = UserRepository;