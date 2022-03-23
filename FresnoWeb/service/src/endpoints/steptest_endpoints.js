const Helpers = require('../helpers/helpers');

// Database repositories
var StepTestRepository = require('../repositories/steptest_repository');

class StepTestEndpoints {
    constructor(rootPath, app, db) {
        this.rootPath = rootPath;
        this.app = app;
        this.db = db;
        this.repository = new StepTestRepository(this.db);
        console.log(`${Helpers.getDateNowString()} HELLO from StepTestEndpoints constructor`);
    }

    endpoints() {
        console.log(`${Helpers.getDateNowString()} Starting enpoints for steptests`);
        const MODULE = "steptest";
        const ALL_STEPTESTS = "getallsteptests";
        const ALL_STEPTESTS_BY_USER = "getsteptestsbyuserid";
        const ONE_STEPTEST = "getsteptestbyid";
        const ONE_STEPTEST_BY_MEASUREMENT = "getsteptestbymeasureid";
        const POST_STEPTEST = "postnewsteptest";
        const UPDATE_STEPTEST = "updatesteptest";
        const DELETE_STEPTEST = "deletesteptestbyid";

        this.app.get(`/${this.rootPath}/${MODULE}/${ALL_STEPTESTS}`, (req, res, next) => {
            this.repository.getAllStepTests(res);
            console.log(`${Helpers.getDateNowString()} request: GET ${ALL_STEPTESTS}. req:${JSON.stringify(req.params)}`);
        });

        this.app.get(`/${this.rootPath}/${MODULE}/${ALL_STEPTESTS_BY_USER}/:id`, (req, res, next) => {
            this.repository.getAllStepTestsByUserId(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: GET ${ALL_STEPTESTS_BY_USER}. req:${JSON.stringify(req.params)}`);
        });

        this.app.get(`/${this.rootPath}/${MODULE}/${ONE_STEPTEST}/:id`, (req, res, next) => {
            this.repository.getStepTestById(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: GET ${ONE_STEPTEST}. req:${JSON.stringify(req.params)}`);
        });

        this.app.get(`/${this.rootPath}/${MODULE}/${ONE_STEPTEST_BY_MEASUREMENT}/:id`, (req, res, next) => {
            this.repository.getStepTestByMeasurementIdId(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: GET ${ONE_STEPTEST_BY_MEASUREMENT}. req:${JSON.stringify(req.params)}`);
        });

        this.app.post(`/${this.rootPath}/${MODULE}/${POST_STEPTEST}/`, (req, res, next) => {
            this.repository.postNewStepTest(res, req.body.UserId, req.body.TestType, req.body.EffortUnit, req.body.StepDuration, req.body.LoadPreset, req.body.Increase, req.body.Temperature, req.body.Weight, req.body.TestDate);
            console.log(`${Helpers.getDateNowString()} request: POST ${POST_STEPTEST}. req: ${JSON.stringify(req.body)}`);
        });

        this.app.delete(`/${this.rootPath}/${MODULE}/${DELETE_STEPTEST}/:id`, (req, res, next) => {
            this.repository.deleteStepTestById(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: DELETE ${DELETE_STEPTEST}. req: ${JSON.stringify(req.params)}`);
        });

        this.app.patch(`/${this.rootPath}/${MODULE}/${UPDATE_STEPTEST}/`, (req, res, next) => {
            this.repository.updateUserById(res, req.body.Id, req.body.UserId, req.body.TestType, req.body.EffortUnit, req.body.StepDuration, req.body.LoadPreset, req.body.Increase, req.body.Temperature, req.body.Weight, req.body.TestDate);
            console.log(`${Helpers.getDateNowString()} request: UPDATE ${UPDATE_STEPTEST}. req: ${JSON.stringify(req.body)}`);
        });
    }
}

module.exports = StepTestEndpoints;