const Helpers = require('../helpers/helpers');
const middleware = require('../middelware/index');
const AuthController = require('../controllers/auth.controller');

class AuthEndpoints {
    constructor(rootPath, app, db) {
        this.rootPath = rootPath;
        this.app = app;
        this.db = db;
        this.verifySignup = new middleware.verifySignUp(db);
        this.authJwt = new middleware.authJwt(db);
        this.controller = new AuthController(db);
        console.log(`${Helpers.getDateNowString()} HELLO from AuthEndpoints constructor`);
    }

    endpoints() {
        console.log(`${Helpers.getDateNowString()} Starting enpoints for auth`);
        const MODULE = "auth";
        const SIGNUP = "signup";
        const SIGNIN = "signin";
        const VERIFY = "verify";

        this.app.use((req, res, next) => {
            res.header(
                "Access-Control-Allow-Headers",
                "x-access-token, Origin, Content-Type, Accept"
            );
            next();
        });

        const checkUsernameAndEmail = (req, res, next) => {
            this.verifySignup.checkDuplicateUsernameOrEmail(req, res, next)
        };
        const checkRolesExisted = (req, res, next) => this.controller.signup(req, res);
        const signin = (req, res) => this.controller.signin(req, res);
        const verify = (req, res) => this.authJwt.verifyToken(req, res);

        this.app.post(`/${this.rootPath}/${MODULE}/${SIGNUP}/`, (req, res, next) => {
            console.log(`${Helpers.getDateNowString()} request: POST ${SIGNUP}. req: ${JSON.stringify(req.body)}`);
            next();
        }, [checkUsernameAndEmail, checkRolesExisted]);

        this.app.post(`/${this.rootPath}/${MODULE}/${SIGNIN}/`, (req, res, next) => {
            console.log(`${Helpers.getDateNowString()} request: POST ${SIGNIN}. req: ${JSON.stringify(req.body)}`);
            next();
        }, signin);

        this.app.get(`/${this.rootPath}/${MODULE}/${VERIFY}/`, (req, res, next) => {
            console.log(`${Helpers.getDateNowString()} request: POST ${VERIFY}. req: ${JSON.stringify(req.body)}`);
            next();
        }, verify);
    };
}

module.exports = AuthEndpoints;