const Helpers = require('../helpers/helpers');

const jwt = require('jsonwebtoken');
const config = require('../config/auth.config');
const SiteUserRepository = require('../repositories/siteuser_repository');
const RoleRepository = require('../repositories/role_repository');
const SiteUserRoleRepository = require('../repositories/siteuserrole_repository');

class AuthJwtMW {
    constructor(db) {
        this.db = db;
        this.userRepository = new SiteUserRepository(db);
        this.roleRepository = new RoleRepository(db);
        this.userRoleRepository = new SiteUserRoleRepository(db);
        console.log(`${Helpers.getDateNowString()} HELLO from AuthJwtMW constructor`);
    }

    verifyToken = (req, res, next) => {
        let token = req.headers["x-access-token"];

        if (!token) {
            return res.status(403).send({
                message: "No token provided!"
            });
        }

        jwt.verify(token, config.secret, (err, decoded) => {
            if (err) {
                return res.status(401).send({
                    message: "Unauthorized!"
                });
            }
            req.userId = decoded.id;
            next();
        });
    };

    async isAdmin(req, res, next) {
        await this.userRepository.findById(req.userId).then(user => {
            this.userRoleRepository.findByUserId(user.id).then(roles => {
                for (let i = 0; i < roles.length; i++) {
                    if (roles[i].name === "Admin") {
                        next();
                        return;
                    }
                }

                res.status(403).send({
                    message: "Require Admin Role!"
                });
                return;
            });
        });
    };

    async isModerator(req, res, next) {
        await this.userRepository.findById(req.userId).then(user => {
            this.userRoleRepository.findByUserId(user.id).then(roles => {
                for (let i = 0; i < roles.length; i++) {
                    if (roles[i].name === "moderator") {
                        next();
                        return;
                    }
                }

                res.status(403).send({
                    message: "Require Moderator Role!"
                });
            });
        });
    };

    async isModeratorOrAdmin(req, res, next) {
        await this.userRepository.findById(req.userId).then(user => {
            this.userRoleRepository.findByUserId(user.id).then(roles => {
                for (let i = 0; i < roles.length; i++) {
                    if (roles[i].name === "Moderator") {
                        next();
                        return;
                    }

                    if (roles[i].name === "Admin") {
                        next();
                        return;
                    }
                }

                res.status(403).send({
                    message: "Require Moderator or Admin Role!"
                });
            });
        });
    };
}

module.exports = AuthJwtMW;