const Helpers = require('./../helpers/helpers');
const DB_TABLE = 'Person';
const INSERT_COLUMNS = 'FirstName, LastName, Email, Street, PostCode, PostCity, BirthDate, Height, Sex, MaxHr';
const FULL_COLUMNS = `Id, ${INSERT_COLUMNS}`;

class PersonRepository {
    constructor(db) {
        this.db = db;
        console.log(`${Helpers.getDateNowString()} HELLO from PersonRepository constructor`);
    }

    // Get all persons
    getAllPersons(res) {
        var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE}`;
        var params = [];
        this.db.all(sql, params, (err, rows) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getAllPersons ERROR: ${err.message}`)
            }
            res.json({
                "message": "success",
                "data": rows
            });
            console.log(`${Helpers.getDateNowString()} getAllPersons returns ${rows.length} rows`);
        });
    };

    // Get person by id
    getPersonById(res, id) {
        var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE} WHERE Id = ?`;
        var params = [id];
        this.db.get(sql, params, (err, row) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getPersonById ERROR: ${err.message}`);
                res.status(400).json({ "error": err.message });
                return;
            }
            res.json({
                "message": "success",
                "data": row
            });
            console.log(`${Helpers.getDateNowString()} getPersonById returns Id ${row.Id}`);

        });
    };

    // Get person by steptest id
    getPersonByStepTestId(res, id) {
        var sql = `SELECT u.Id, u.FirstName, u.LastName, u.Email, u.Street, u.PostCode, u.PostCity, u.BirthDate, u.Height, u.Sex, u.MaxHr FROM ${DB_TABLE} u JOIN StepTest st ON st.PersonId = u.Id WHERE st.Id = ?`;
        var params = [id];
        this.db.get(sql, params, (err, row) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getPersonByStepTestId ERROR: ${err.message}`);
                res.status(400).json({ "error": err.message });
                return;
            }
            res.json({
                "message": "success",
                "data": row
            });
            console.log(`${Helpers.getDateNowString()} getPersonByStepTestId returns Id ${row.Id}`);

        });
    };

    // New person
    postNewPerson(res, firstname, lastname, email, street, postcode, postcity, birthdate, height, sex, maxhr) {
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

        var sql = `INSERT INTO ${DB_TABLE} (${INSERT_COLUMNS}) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)`;
        var params = [data.FirstName, data.LastName, data.Email, data.Street, data.PostCode, data.PostCity, data.BirthDate, data.Height, data.Sex, data.MaxHr];

        this.db.serialize(() => {
            this.db.run(sql, params, (err) => {
                if (err) {
                    console.error(`${Helpers.getDateNowString()} postNewPerson ERROR: ${err.message}`);
                    res.status(400).json({ "error": err.message });
                    return;
                }
            });

            this.db.get(`SELECT ${FULL_COLUMNS} FROM ${DB_TABLE} WHERE Id = (SELECT seq FROM sqlite_sequence WHERE name = '${DB_TABLE}')`, [], (err, row) => {
                if (err) {
                    console.error(`${Helpers.getDateNowString()} postNewPerson ERROR: ${err.message}`);
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

    // Update person
    updatePersonById(res, id, firstname, lastname, email, street, postcode, postcity, birthdate, height, sex, maxhr) {
        var data = { Id: id, FirstName: firstname, LastName: lastname, Email: email, Street: street, PostCode: postcode, PostCity: postcity, Height: height, BirthDate: birthdate, Sex: sex, MaxHr: maxhr };

        var sql = `UPDATE ${DB_TABLE} SET FirstName = COALESCE(?, FirstName), LastName = COALESCE(?, LastName), Email = COALESCE(?, Email), Street = COALESCE(?, Street), PostCode = COALESCE(?, PostCode), PostCity = COALESCE(?, PostCity), BirthDate = COALESCE(?, BirthDate), Height = COALESCE(?, Height), Sex = COALESCE(?, Sex), MaxHr = COALESCE(?, MaxHr) WHERE Id = ?`;
        var params = [data.FirstName, data.LastName, data.Email, data.Street, data.PostCode, data.PostCity, data.BirthDate, data.Height, data.Sex, data.MaxHr, data.Id];
        this.db.run(sql, params, (err, result) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} updatePersonById ERROR: ${err.message}`);
                res.status(400).json({ "error": res.message })
                return;
            }
            res.json({
                message: "success",
                data: data,
                changes: this.changes
            });
            console.log(`${Helpers.getDateNowString()} updatePersonById: Id: ${data.Id} => Update: ${JSON.stringify(data)}`);
        });
    };

    // Delete Person
    deletePersonById(res, id) {
        var sql = `DELETE FROM ${DB_TABLE} WHERE Id = ?`;
        var params = [id];
        this.db.run(sql, params, (err, result) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} deletePersonById ERROR: ${err.message}`);
                res.status(400).json({ "error": res.message })
                return;
            }
            res.json({
                "message": "deleted",
                "id": id
            });
            console.log(`${Helpers.getDateNowString()} deletePersonById: ${id}`);
        });
    };
}

module.exports = PersonRepository;