"use strict";

module.exports = function (callback, param) {
    const puppeteer = require("puppeteer");
    const moment = require("moment");
    const fs = require("fs");
    const path = require("path");


   
    let url = param.url;
    let vechile = param.vechile;
 
    let fileName = vechile;



    (async () => {
        try {
            const browser = await puppeteer.launch({
                //devtools: true
                headless: true,
                args: ["--no-sandbox"]
            });
            const page = await browser.newPage();

            const today = new Date();
            let dd = today.getDate();
            let mm = today.getMonth() + 1; //January is 0!
            const yyyy = today.getFullYear();

            if (dd < 10) {
                dd = "0" + dd;
            }

            if (mm < 10) {
                mm = "0" + mm;
            }

            page.on("error", async error => {
                const dir = path.resolve(__dirname, "../Error");
                if (!fs.existsSync(dir)) {
                    fs.mkdirSync(dir);
                }
                await page.screenshot({
                    path: `${dir}/screenshot${mm +
                        "-" +
                        dd +
                        "-" +
                        yyyy +
                        "-" +
                        today.getTime()}.png`
                });
                callback(error);
            });
            await page.setUserAgent(
                "Mozilla/5.0 (iPhone; CPU iPhone OS 9_0_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13A404 Safari/601.1"
            );

            await page.goto(url, {
                waitUntil: "networkidle0",
                timeout: 30000000
            });

            await page.waitFor(() => {
                const modal = document.querySelector("body.modal-open");
                if (modal) {
                    const modalText = document.querySelector(
                        "body > div.fade.error-new-modal.modal.show > div > div > div.modal-body > p"
                    );
                    return modalText.textContent == "Sorry, there are no cars available.";
                }
                return !modal;
            });
            await scrollToBottom(page);
             

            const screenshotFolder = path.resolve(__dirname, "../ScreenShot");

            if (!fs.existsSync(screenshotFolder)) {
                fs.mkdirSync(screenshotFolder);
            }

            const dir = screenshotFolder + "/" + mm + "-" + dd + "-" + yyyy;

            if (!fs.existsSync(dir)) {
                fs.mkdirSync(dir);
            }

            await page.screenshot({
                path: `${dir}/${fileName}.png`,
                fullPage: true
            });
            await browser.close();
            callback(null);
        } catch (error) {
            callback(error);
        }
    })();


    async function scrollToBottom(page) {
        const distance = 200;
        const delay = 500;
        while (await page.evaluate(() => document.scrollingElement.scrollTop + window.innerHeight < document.scrollingElement.scrollHeight)) {
            await page.evaluate((y) => { document.scrollingElement.scrollBy(0, y); }, distance);
            await page.waitFor(delay);
        }
    }
    async function writeData(result) {
        fs.appendFileSync("C:\\result.txt", result);
        fs.appendFileSync("C:\\result.txt", "\n");
    }
};
