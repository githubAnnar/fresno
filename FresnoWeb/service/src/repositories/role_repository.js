const Helpers = require('./../helpers/helpers');
const DB_TABLE = 'Role';
const INSERT_COLUMNS = 'Name, CreatedAt, UpdatedAt';
const FULL_COLUMNS = `Id, ${INSERT_COLUMNS}`;

class RoleRepository {
    constructor(db) {
        this.db = db;
        console.log(`${Helpers.getDateNowString()} HELLO from RoleRepository constructor`);
    }

    // Get all roles
    async findAll() {
        return new Promise(resolve => {
            var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE}`;
            var params = [];
            this.db.all(sql, params, (err, rows) => {
                if (err) {
                    return console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                }

                console.log(`${Helpers.getDateNowString()} findAll returns ${JSON.stringify(rows)}`);
                resolve(rows);
            });
        })
    }

    getAllRoles(res) {
        var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE}`;
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
            console.log(`${Helpers.getDateNowString()} getAllRoles returns ${rows.length} rows`);
        });
    }

    // Get role by Id
    async findById(id) {
        return new Promise(resolve => {
            var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE} WHERE Id = ?`;
            var params = [id];
            this.db.get(sql, params, (err, row) => {
                if (err) {
                    return console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                }

                console.log(`${Helpers.getDateNowString()} findById returns Id ${row.Id}`);
                resolve(row);
            });
        });
    }

    getRoleById(res, id) {
        var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE} WHERE Id = ?`;
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
            console.log(`${Helpers.getDateNowString()} getRoleById returns Id ${row.Id}`);
        });
    }

    // Get role by name
    async findByName(name) {
        return new Promise(resolve => {
            var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE} WHERE Name = ?`;
            var params = [id];
            this.db.get(sql, params, (err, row) => {
                if (err) {
                    return console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                }

                console.log(`${Helpers.getDateNowString()} findByName returns Id ${row.Id}`);
                resolve(row);
            });
        });
    }

    async findByNames(names) {
        console.log(`${Helpers.getDateNowString()} names=`, names);
        let data = [];
        return new Promise(resolve => {
            var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE} WHERE Name IN (${names.map(n => { return '?' }).join(',')})`;
            this.db.all(sql, names, (err, rows) => {
                if (err) {
                    return console.error(`${Helpers.getDateNowString()} ERROR: ${err.message}`);
                }
                rows.forEach((row) => {
                    data.push(row);
                });
                console.log(`${Helpers.getDateNowString()} findByNames returns ${JSON.stringify(data)}`);
                resolve(data);
            });
        });
    }
}

module.exports = RoleRepository;