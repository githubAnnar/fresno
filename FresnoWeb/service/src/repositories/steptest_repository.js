const Helpers = require('./../helpers/helpers');
const DB_TABLE = 'StepTest';
const INSERT_COLUMNS = 'PersonId, TestType, EffortUnit, StepDuration, LoadPreset, Increase, Temperature, Weight, TestDate';
const FULL_COLUMNS = `Id, ${INSERT_COLUMNS}`;

class StepTestRepository {
    constructor(db) {
        this.db = db;
        console.log(`${Helpers.getDateNowString()} HELLO from StepTestRepository constructor`);
    }

    // Get All Step Tests
    getAllStepTests(res) {
        var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE}`;
        var params = [];
        this.db.all(sql, params, (err, rows) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getAllStepTests ERROR: ${err.message}`)
            }
            res.json({
                "message": "success",
                "data": rows
            });
            console.log(`${Helpers.getDateNowString()} getAllStepTests returns ${rows.length} rows`);
        });
    };

    getAllStepTestsEx(res) {
        var sql = `SELECT st.Id, st.PersonId, st.TestType, st.EffortUnit, st.StepDuration, st.LoadPreset, st.Increase, st.Temperature, st.Weight, st.TestDate, p.FirstName, p.LastName FROM ${DB_TABLE} st JOIN Person p ON st.PersonId = p.Id`;
        var params = [];
        this.db.all(sql, params, (err, rows) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getAllStepTestsEx ERROR: ${err.message}`)
            }
            res.json({
                "message": "success",
                "data": rows
            });
            console.log(`${Helpers.getDateNowString()} getAllStepTestsEx returns ${rows.length} rows`);
        });
    }

    // Get all step tests by person id
    getAllStepTestsByPersonId(res, id) {
        var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE} WHERE PersonId = ?`;
        var params = [id];
        this.db.all(sql, params, (err, rows) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getAllStepTestsByPersonId ERROR: ${err.message}`)
            }
            res.json({
                "message": "success",
                "data": rows
            });
            console.log(`${Helpers.getDateNowString()} getAllStepTestsByPersonId returns ${rows.length} rows`);
        });
    };

    // Get Step Test by Measurement id
    getStepTestByMeasurementIdId(res, id) {
        var sql = `SELECT st.Id, st.UserId, st.TestType, st.EffortUnit, st.StepDuration, st.LoadPreset, st.Increase, st.Temperature, st.Weight, st.TestDate FROM ${DB_TABLE} st JOIN Measurement m ON m.StepTestId = st.Id WHERE m.Id = ?`;
        var params = [id];
        this.db.get(sql, params, (err, row) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getStepTestByMeasurementIdId ERROR: ${err.message}`);
                res.status(400).json({ "error": err.message });
                return;
            }
            res.json({
                "message": "success",
                "data": row
            });
            console.log(`${Helpers.getDateNowString()} getStepTestByMeasurementIdId returns Id ${row.Id}`);

        });
    };

    // Get StepTest By Id
    getStepTestById(res, id) {
        var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE} WHERE Id = ?`;
        var params = [id];
        this.db.get(sql, params, (err, row) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getStepTestById ERROR: ${err.message}`);
                res.status(400).json({ "error": err.message });
                return;
            }

            res.json({
                "message": "success",
                "data": row
            });
            console.log(`${Helpers.getDateNowString()} getStepTestById returns Id ${row.Id}`);
        });
    };

    // Get StepTest text by Id
    getStepTestTextById(res, id) {
        var sql = `SELECT st.TestDate, p.FirstName, p.LastName FROM StepTest st JOIN Person p ON st.PersonId = p.Id WHERE st.Id = ?`;
        var params = [id];
        this.db.get(sql, params, (err, row) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getStepTestTextById ERROR: ${err.message}`);
                res.status(400).json({ "error": err.message });
                return;
            }

            res.json({
                "message": "success",
                "data": row
            });
            console.log(`${Helpers.getDateNowString()} getStepTestTextById returns ${JSON.stringify(row)}`);
        });
    }

    // New StepTest
    postNewStepTest(res, userid, testtype, effortunit, stepduration, loadpreset, increase, temperature, weight, testdate) {
        var errors = [];

        if (!userid) {
            errors.push('No user specified');
        }

        if (!testtype) {
            errors.push('No testtype specified');
        }

        if (!effortunit) {
            errors.push('No efforttype specified');
        }

        if (!stepduration) {
            errors.push('No stepduration specified');
        }

        if (!loadpreset) {
            errors.push('No loadpreset specified');
        }

        if (!increase) {
            errors.push('No increase specified');
        }

        if (errors.length) {
            console.error(`${Helpers.getDateNowString()} ERROR: ${errors.join(", ")}`);
            res.status(400).json({ "error": errors.join(", ") });
            return;
        }

        var data = { UserId: userid, TestType: testtype, EffortUnit: effortunit, StepDuration: stepduration, LoadPreset: loadpreset, Increase: increase, Temperature: temperature, Weight: weight, TestDate: testdate };

        var sql = `INSERT INTO ${DB_TABLE} (${INSERT_COLUMNS}) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)`;
        var params = [data.UserId, data.TestType, data.EffortUnit, data.StepDuration, data.LoadPreset, data.Increase, data.Temperature, data.Weight, data.TestDate];

        this.db.serialize(() => {
            this.db.run(sql, params, (err) => {
                if (err) {
                    console.error(`${Helpers.getDateNowString()} postNewStepTest ERROR: ${err.message}`);
                    res.status(400).json({ "error": err.message });
                    return;
                }
            });

            this.db.get(`SELECT ${FULL_COLUMNS} FROM ${DB_TABLE} WHERE Id = (SELECT seq FROM sqlite_sequence WHERE name = '${DB_TABLE}')`, [], (err, row) => {
                if (err) {
                    console.error(`${Helpers.getDateNowString()} postNewStepTest ERROR: ${err.message}`);
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

    // Update StepTest
    updateStepTestById(res, id, userid, testtype, effortunit, stepduration, loadpreset, increase, temperature, weight, testdate) {
        var data = { Id: id, UserId: userid, TestType: testtype, EffortUnit: effortunit, StepDuration: stepduration, LoadPreset: loadpreset, Increase: increase, Temperature: temperature, Weight: weight, TestDate: testdate };

        var sql = `UPDATE ${DB_TABLE} SET UserId = COALESCE(?, UserId), TestType = COALESCE(?, TestType), EffortUnit = COALESCE(?, EffortUnit), StepDuration = COALESCE(?, StepDuration), LoadPreset = COALESCE(?, LoadPreset), Increase = COALESCE(?, Increase), Temperature = COALESCE(?, Temperature), Weight = COALESCE(?, Weight), TestDate = COALESCE(?, TestDate) WHERE Id = ?`;
        var params = [data.UserId, data.TestType, data.EffortUnit, data.StepDuration, data.LoadPreset, data.Increase, data.Temperature, data.Weight, data.TestDate, data.Id];
        this.db.run(sql, params, (err, result) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} updateStepTestById ERROR: ${err.message}`);
                res.status(400).json({ "error": res.message })
                return;
            }
            res.json({
                message: "success",
                data: data,
                changes: this.changes
            });
            console.log(`${Helpers.getDateNowString()} updateStepTestById: Id: ${data.Id} => Update: ${JSON.stringify(data)}`);
        });
    };

    // Delete StepTest
    deleteStepTestById(res, id) {
        var sql = `DELETE FROM ${DB_TABLE} WHERE Id = ?`;
        var params = [id];
        this.db.run(sql, params, (err, result) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} deleteStepTestById ERROR: ${err.message}`);
                res.status(400).json({ "error": res.message })
                return;
            }
            res.json({
                "message": "deleted",
                "id": id
            });
            console.log(`${Helpers.getDateNowString()} deleteStepTestById: ${id}`);
        });
    };
}

module.exports = StepTestRepository;