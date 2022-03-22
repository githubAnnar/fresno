const Helpers = require("../helpers/helpers.js");

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
        const POST_USER = "postnewuser";
        const UPDATE_USER = "updateuserbyid";
        const DELETE_USER = "deleteuserbyid";
    }
}

module.exports = UserEndpoints;