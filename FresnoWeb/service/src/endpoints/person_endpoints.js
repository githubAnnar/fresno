const Helpers = require('../helpers/helpers');

// Database repositories
var PersonRepository = require('../repositories/person_repository');

class PersonEndpoints {
    constructor(rootPath, app, db) {
        this.rootPath = rootPath;
        this.app = app;
        this.db = db;
        this.repository = new PersonRepository(this.db);
        console.log(`${Helpers.getDateNowString()} HELLO from PersonEndpoints constructor`);
    }

    endpoints() {
        console.log(`${Helpers.getDateNowString()} Starting enpoints for person`);
        const MODULE = "person";
        const ALL_PERSONS = "getallpersons";
        const ONE_PERSON = "getpersonbyid";
        const ONE_PERSON_BY_STEPTEST = "getpersonbysteptestid";
        const POST_USER = "postnewperson";
        const UPDATE_PERSON = "updateperson";
        const DELETE_PERSON = "deletepersonbyid";
        const ONE_PERSON_NAME = 'getpersonnamebyid';
        const ONE_PERSON_FULLNAME = 'getpersonfullnamebyid';

        this.app.get(`/${this.rootPath}/${MODULE}/${ALL_PERSONS}`, (req, res, next) => {
            this.repository.getAllPersons(res);
            console.log(`${Helpers.getDateNowString()} request: GET ${ALL_PERSONS}. req:${JSON.stringify(req.params)}`);
        });

        this.app.get(`/${this.rootPath}/${MODULE}/${ONE_PERSON}/:id`, (req, res, next) => {
            this.repository.getPersonById(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: GET ${ONE_PERSON}. req:${JSON.stringify(req.params)}`);
        });

        this.app.get(`/${this.rootPath}/${MODULE}/${ONE_PERSON_BY_STEPTEST}/:id`, (req, res, next) => {
            this.repository.getPersonByStepTestId(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: GET ${ONE_PERSON_BY_STEPTEST}. req:${JSON.stringify(req.params)}`);
        });

        this.app.get(`/${this.rootPath}/${MODULE}/${ONE_PERSON_NAME}/:id`, (req, res, next) => {
            this.repository.getPersonNameById(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: GET ${ONE_PERSON_NAME}. req:${JSON.stringify(req.params)}`);
        });

        this.app.post(`/${this.rootPath}/${MODULE}/${POST_USER}/`, (req, res, next) => {
            this.repository.postNewPerson(res, req.body.FirstName, req.body.LastName, req.body.Email, req.body.Street, req.body.PostCode, req.body.PostCity, req.body.BirthDate, req.body.Height, req.body.Sex, req.body.MaxHr);
            console.log(`${Helpers.getDateNowString()} request: POST ${POST_USER}. req: ${JSON.stringify(req.body)}`);
        });

        this.app.delete(`/${this.rootPath}/${MODULE}/${DELETE_PERSON}/:id`, (req, res, next) => {
            this.repository.deletePersonById(res, req.params.id);
            console.log(`${Helpers.getDateNowString()} request: DELETE ${DELETE_PERSON}. req: ${JSON.stringify(req.params)}`);
        });

        this.app.patch(`/${this.rootPath}/${MODULE}/${UPDATE_PERSON}/`, (req, res, next) => {
            this.repository.updatePersonById(res, req.body.Id, req.body.FirstName, req.body.LastName, req.body.Email, req.body.Street, req.body.PostCode, req.body.PostCity, req.body.BirthDate, req.body.Heigh, req.body.Sex, req.body.MaxHr);
            console.log(`${Helpers.getDateNowString()} request: UPDATE ${UPDATE_PERSON}. req: ${JSON.stringify(req.body)}`);
        });
    }
}

module.exports = PersonEndpoints;