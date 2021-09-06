"use strict";

module.exports = function (callback, param) {
    const puppeteer = require("puppeteer");
    const moment = require("moment");
    const fs = require("fs");
    const path = require("path");

    let url = param.location;
    //switch (param.location) {
    //    case "Parramatta, New South Wales, Australia":
    //        url =
    //            "https://booking.vroomvroomvroom.com.au/au/results/2018-02-06/10:00/2017-02-07/10:00/-33.815127,151.003156,2/-33.815127,151.003156,2/Parramatta,%20New%20South%20Wales,%20Australia/Parramatta,%20New%20South%20Wales,%20Australia/PK/30?radius=5";
    //        break;
    //    case "Sydney Airport, New South Wales, Australia":
    //        url =
    //            "https://booking.vroomvroomvroom.com.au/au/results/2018-02-06/10:00/2017-02-07/10:00/-33.93287300006831,151.17840920268554,1/-33.93287300006831,151.17840920268554,1/Sydney%20Airport,%20New%20South%20Wales,%20Australia/Sydney%20Airport,%20New%20South%20Wales,%20Australia/PK/30?radius=5";
    //        break;
    //    case "Liverpool, New South Wales, Australia":
    //        url =
    //            "https://booking.vroomvroomvroom.com.au/au/results/2018-02-06/10:00/2017-02-07/10:00/-33.920229908684945,150.92354530151374,2/-33.920229908684945,150.92354530151374,2/Liverpool,%20New%20South%20Wales,%20Australia/Liverpool,%20New%20South%20Wales,%20Australia/PK/30?radius=5";
    //        break;
    //    case "Sutherland NSW, Australia":
    //        url =
    //            "https://booking.vroomvroomvroom.com.au/au/results/2018-02-06/10:00/2017-02-07/10:00/-34.03314,151.05830000000003,2/-34.03314,151.05830000000003,2/Sutherland%20NSW,%20Australia/Sutherland%20NSW,%20Australia/PK/30";
    //        break;
    //}

    url = url.replace(
        "2020-06-29",
        moment()
            .add(param.pickupDate, "days")
            .format("YYYY-MM-DD")
    );

    url = url.replace(
        "2020-07-02",
        moment()
            .add(param.returnDate, "days")
            .format("YYYY-MM-DD")
    );

    (async () => {
        try {
            const browser = await puppeteer.launch({
                devtools: true
                // headless: true,
                //  args: ["--no-sandbox"]
            });
            const page = await browser.newPage();
            debugger;

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
            const result = await page.evaluate(() => {
                const ul = document.querySelector("ul.list-unstyled");
                const models = [];

                if (ul) {
                    const getNumeric = str => Number(str.replace(/[^0-9\.]+/g, ""));

                    for (let index = 0; index < ul.childElementCount; index++) {
                        try {
                            const li = ul.children[index];
                            const a = li.children[0];
                            const div = a.children[0];


                            const detailUrl = a.href;
                            const Supplier = div.children[0].children[0].children[0].src;
                            const logo = div.children[0].children[1].children[0].src;
                            const vehicle = div.children[1].children[1].children[0].textContent.trim();

                            const type = div.children[1].children[1].children[2].textContent.trim();

                            const numberOfSeat = getNumeric(
                                document
                                    .querySelector(`.feature-item[data-for='seats-${index}']`)
                                    .textContent.trim()
                            ); // No of Seats
                            const numberOfDoor = getNumeric(
                                document
                                    .querySelector(`.feature-item[data-for='door-${index}']`)
                                    .textContent.trim()
                            ); // No of Door
                            const numberOfBag = document
                                .querySelector(`.feature-item[data-for='bags-${index}']`)
                                .textContent.trim(); // No of Bags
                            const transmission = document
                                .querySelector(`.feature-item[data-for='transmission-${index}']`)
                                .textContent.trim(); // No of Transmission

                            const totalPrice = getNumeric(
                                document
                                    .querySelectorAll(`.total-price`)
                                [index].textContent.trim()
                            );
                            const perDayPrice = getNumeric(
                                document
                                    .querySelectorAll(`.perdayprice`)
                                [index].textContent.trim()
                            );
                            const paymentType = document
                                .querySelectorAll(`.payment-type`)
                            [index].textContent.trim();
                            const currency = document
                                .getElementsByClassName("total-price")
                            [index].children[0].textContent.trim();

                            console.log(logo);
                            console.log(Supplier);
                            console.log(vehicle);
                            console.log(totalPrice);
                            console.log(type); //company logo picture
                            console.log(transmission); //Car picture
                            console.log(numberOfSeat);
                            console.log(numberOfDoor);
                            console.log(numberOfBag);
                            console.log(perDayPrice);
                            console.log(paymentType);
                            console.log(currency);

                            // debugger;
                            models.push({
                                vehicle,
                                totalPrice,
                                type,
                                transmission,
                                logo,
                                Supplier,
                                numberOfSeat,
                                numberOfDoor,
                                numberOfBag,
                                perDayPrice,
                                paymentType,
                                currency,
                                detailUrl
                            });

                        } catch{ }

                    }
                }
                console.log(models);
                return models;
            });

            //const screenshotFolder = path.resolve(__dirname, "../ScreenShot");

            //if (!fs.existsSync(screenshotFolder)) {
            //    fs.mkdirSync(screenshotFolder);
            //}

            //const dir = screenshotFolder + "/" + mm + "-" + dd + "-" + yyyy;

            //if (!fs.existsSync(dir)) {
            //    fs.mkdirSync(dir);
            //}

            //await page.screenshot({
            //    path: `${dir}/${param.id}.png`,
            //    fullPage: true
            //});
            await browser.close();
            callback(null, result);
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
