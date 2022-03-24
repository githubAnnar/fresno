const Helpers = require('./../helpers/helpers');
const DB_TABLE = 'Measurement';
const INSERT_COLUMNS = 'Sequence, StepTestId, HeartRate, Lactate, Load, InCalculation';
const FULL_COLUMNS = `Id, ${INSERT_COLUMNS}`;

class MeasurementRepository {
    constructor(db) {
        this.db = db;
        console.log(`${Helpers.getDateNowString()} HELLO from MeasurementRepository constructor`);
    }

    // Get All Measurements
    getAllMeasurements(res) {
        var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE}`;
        var params = [];
        this.db.all(sql, params, (err, rows) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getAllMeasurements ERROR: ${err.message}`)
            }
            res.json({
                "message": "success",
                "data": rows
            });
            console.log(`${Helpers.getDateNowString()} getAllMeasurements returns ${rows.length} rows`);
        });
    };

    // Get all measurements by steptest id
    getAllMeasurementsByStepTestId(res, id) {
        var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE} WHERE StepTestId = ?`;
        var params = [id];
        this.db.all(sql, params, (err, rows) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getAllMeasurementsByStepTestId ERROR: ${err.message}`)
            }
            res.json({
                "message": "success",
                "data": rows
            });
            console.log(`${Helpers.getDateNowString()} getAllMeasurementsByStepTestId returns ${rows.length} rows`);
        });
    };

    // Get Measurement By Id
    getMeasurementById(res, id) {
        var sql = `SELECT ${FULL_COLUMNS} FROM ${DB_TABLE} WHERE Id = ?`;
        var params = [id];
        this.db.get(sql, params, (err, row) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} getMeasurementById ERROR: ${err.message}`);
                res.status(400).json({ "error": err.message });
                return;
            }
            res.json({
                "message": "success",
                "data": row
            });
            console.log(`${Helpers.getDateNowString()} getMeasurementById returns Id ${row.Id}`);

        });
    };

    // New Measurement
    postNewMeasurement(res, sequence, steptestid, heartrate, lactate, load, incalculation) {
        var errors = [];

        if (!sequence) {
            errors.push('No sequence specified');
        }

        if (!steptestid) {
            errors.push('No steptestid specified');
        }

        if (!heartrate) {
            errors.push('No heartrate specified');
        }

        if (!lactate) {
            errors.push('No lactate specified');
        }

        if (!load) {
            errors.push('No load specified');
        }

        if (errors.length) {
            console.error(`${Helpers.getDateNowString()} ERROR: ${errors.join(", ")}`);
            res.status(400).json({ "error": errors.join(", ") });
            return;
        }

        var data = { Sequence: sequence, StepTestId: steptestid, HeartRate: heartrate, Lactate: lactate, Load: load, InCalculation: incalculation };

        var sql = `INSERT INTO ${DB_TABLE} (${INSERT_COLUMNS}) VALUES (?, ?, ?, ?, ?, ?)`;
        var params = [data.Sequence, data.StepTestId, data.HeartRate, data.Lactate, data.Load, data.InCalculation];

        this.db.serialize(() => {
            this.db.run(sql, params, (err) => {
                if (err) {
                    console.error(`${Helpers.getDateNowString()} postNewMeasurement ERROR: ${err.message}`);
                    res.status(400).json({ "error": err.message });
                    return;
                }
            });

            this.db.get(`SELECT ${FULL_COLUMNS} FROM ${DB_TABLE} WHERE Id = (SELECT seq FROM sqlite_sequence WHERE name = '${DB_TABLE}')`, [], (err, row) => {
                if (err) {
                    console.error(`${Helpers.getDateNowString()} postNewMeasurement ERROR: ${err.message}`);
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

    // Update Measurement
    updateMeasurementById(res, id, sequence, steptestid, heartrate, lactate, load, incalculation) {
        var data = { Id: id, Sequence: sequence, StepTestId: steptestid, HeartRate: heartrate, Lactate: lactate, Load: load, InCalculation: incalculation };

        var sql = `UPDATE ${DB_TABLE} SET Sequence = COALESCE(?, Sequence), StepTestId = COALESCE(?, StepTestId), HeartRate = COALESCE(?, HeartRate), Lactate = COALESCE(?, Lactate), Load = COALESCE(?, Load), InCalculation = COALESCE(?, InCalculation) WHERE Id = ?`;
        var params = [data.Sequence, data.StepTestId, data.HeartRate, data.Lactate, data.Load, data.InCalculation, data.Id];
        this.db.run(sql, params, (err, result) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} updateMeasurementById ERROR: ${err.message}`);
                res.status(400).json({ "error": res.message })
                return;
            }
            res.json({
                message: "success",
                data: data,
                changes: this.changes
            });
            console.log(`${Helpers.getDateNowString()} updateSupdateMeasurementByIdtepTestById: Id: ${data.Id} => Update: ${JSON.stringify(data)}`);
        });
    };

    // Delete Measurement
    deleteMeasurementById(res, id) {
        var sql = `DELETE FROM ${DB_TABLE} WHERE Id = ?`;
        var params = [id];
        this.db.run(sql, params, (err, result) => {
            if (err) {
                console.error(`${Helpers.getDateNowString()} deleteMeasurementById ERROR: ${err.message}`);
                res.status(400).json({ "error": res.message })
                return;
            }
            res.json({
                "message": "deleted",
                "id": id
            });
            console.log(`${Helpers.getDateNowString()} deleteMeasurementById: ${id}`);
        });
    };
}

module.exports = MeasurementRepository;