const Helpers = require('../helpers/helpers');

// Database repositories
var UserRepository = require('../repositories/user_repository');

class UserEndpoints {
    constructor(rootPath, app, db) {
        this.rootPath = rootPath;
        this.app = app;
        this.db = db;
        this.repository = new UserRepository(this.db);
        console.log(`${Helpers.getDateNowString()} HELLO from UserEndpoints constructor`);
    }

    endpoints() {
        console.log(`${Helpers.getDateNowString()} Starting enpoints for user`);
        const MODULE = "user";
        const ALL_USERS = "getallusers";
        const ONE_USER = "getuserbyid";
        const ONE_USER_BY_STEPTEST = "getuserbysteptestid";
        const POST_USER = "postnewuser";
        const UPDATE_USER = "updateuser";
        const DELETE_USER = "deleteuserbyid";

        this.app.get(`/${this.rootPath}/${MODULE}/${ALL_USERS}`, (req, res, next) => {
            this.repository.getAllUsers(res);
            console.log(`${Helpers.getDateNowString()} request: GET ${ALL_USERS}. req:${JSON.stringify(req.params)}`);
        });

        this.app.get(`/${this.rootPath}/${MODULE}/${ONE_USER}/:id`, (req, res, next) => {
            this.repository.getUserById(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: GET ${ONE_USER}. req:${JSON.stringify(req.params)}`);
        });

        this.app.get(`/${this.rootPath}/${MODULE}/${ONE_USER_BY_STEPTEST}/:id`, (req, res, next) => {
            this.repository.getUserByStepTestId(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: GET ${ONE_USER_BY_STEPTEST}. req:${JSON.stringify(req.params)}`);
        });

        this.app.post(`/${this.rootPath}/${MODULE}/${POST_USER}/`, (req, res, next) => {
            this.repository.postNewUser(res, req.body.FirstName, req.body.LastName, req.body.Email, req.body.Street, req.body.PostCode, req.body.PostCity, req.body.BirthDate, req.body.Height, req.body.Sex, req.body.MaxHr);
            console.log(`${Helpers.getDateNowString()} request: POST ${POST_USER}. req: ${JSON.stringify(req.body)}`);
        });

        this.app.delete(`/${this.rootPath}/${MODULE}/${DELETE_USER}/:id`, (req, res, next) => {
            this.repository.deleteUserById(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: DELETE ${DELETE_USER}. req: ${JSON.stringify(req.params)}`);
        });

        this.app.patch(`/${this.rootPath}/${MODULE}/${UPDATE_USER}/`, (req, res, next) => {
            this.repository.updateUserById(res, req.body.Id, req.body.FirstName, req.body.LastName, req.body.Email, req.body.Street, req.body.PostCode, req.body.PostCity, req.body.BirthDate, req.body.Heigh, req.body.Sex, req.body.MaxHr);
            console.log(`${Helpers.getDateNowString()} request: UPDATE ${UPDATE_USER}. req: ${JSON.stringify(req.body)}`);
        });
    }
}

module.exports = UserEndpoints;