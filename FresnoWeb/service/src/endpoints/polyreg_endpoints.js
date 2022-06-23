const Helpers = require('../helpers/helpers');

// PolyReg Calculations
const { createModel } = require('../polyreg');

class PolyRegEndpoints {
    constructor(rootPath, app) {
        this.rootPath = rootPath;
        this.app = app;
        console.log(`${Helpers.getDateNowString()} HELLO from PolyRegEndpoints constructor`);
    }

    endpoints() {
        console.log(`${Helpers.getDateNowString()} Starting enpoints for polyreg`);
        const MODULE = 'polyreg';
        const FITLACTATE = 'fitlactate';
        const FITHEART = 'fitheart';

        this.app.post(`/${this.rootPath}/${MODULE}/${FITLACTATE}/`, (req, res, next) => {
            const model = createModel();
            model.fit(req.body.data, [3]);
            res.json({
                'message': 'success',
                'data': model.expressions()
            });
        });

        this.app.post(`/${this.rootPath}/${MODULE}/${FITHEART}/`, (req, res, next) => {
            const model = createModel();
            model.fit(req.bod.data[1]);
            res.json({
                'message': 'success',
                'data': model.expressions()
            });
        });
    }
}

module.exports = PolyRegEndpoints;