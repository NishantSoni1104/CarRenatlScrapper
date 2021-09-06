const puppeteer = require("puppeteer");
const moment = require("moment");
const fs = require('fs');
const path = require('path');

const getData = async (param) => {
    try {
        let url = "";
        switch (param.location) {
            case "Parramatta, New South Wales, Australia":
                url = "https://booking.vroomvroomvroom.com.au/au/results/2018-02-06/10:00/2017-02-07/10:00/-33.815127,151.003156,2/-33.815127,151.003156,2/Parramatta,%20New%20South%20Wales,%20Australia/Parramatta,%20New%20South%20Wales,%20Australia/PK/30?radius=5";
                break;
            case "Sydney Airport, New South Wales, Australia":
                url = "https://booking.vroomvroomvroom.com.au/au/results/2018-02-06/10:00/2017-02-07/10:00/-33.93287300006831,151.17840920268554,1/-33.93287300006831,151.17840920268554,1/Sydney%20Airport,%20New%20South%20Wales,%20Australia/Sydney%20Airport,%20New%20South%20Wales,%20Australia/PK/30?radius=5";
                break
            case "Liverpool, New South Wales, Australia":
                url = "https://booking.vroomvroomvroom.com.au/au/results/2018-02-06/10:00/2017-02-07/10:00/-33.920229908684945,150.92354530151374,2/-33.920229908684945,150.92354530151374,2/Liverpool,%20New%20South%20Wales,%20Australia/Liverpool,%20New%20South%20Wales,%20Australia/PK/30?radius=5";
                break;
            case "Sutherland NSW, Australia":
                url = "https://booking.vroomvroomvroom.com.au/au/results/2018-02-06/10:00/2017-02-07/10:00/-34.03314,151.05830000000003,2/-34.03314,151.05830000000003,2/Sutherland%20NSW,%20Australia/Sutherland%20NSW,%20Australia/PK/30";
                break;
        }

        url = url.replace(
            "2018-02-06",
            moment()
                .add(param.pickupDate, "days")
                .format("YYYY-MM-DD")
        );

        url = url.replace(
            "2017-02-07",
            moment()
                .add(param.returnDate, "days")
                .format("YYYY-MM-DD")
        );

        const browser = await puppeteer.launch({ headless: true, args: ['--no-sandbox'] });
        const page = await browser.newPage();


        const today = new Date();
        let dd = today.getDate();
        let mm = today.getMonth() + 1; //January is 0!
        const yyyy = today.getFullYear();

        if (dd < 10) {
            dd = '0' + dd;
        }

        if (mm < 10) {
            mm = '0' + mm;
        }

        page.on('error', async error => {

            const dir = path.resolve(__dirname, "../Error");
            if (!fs.existsSync(dir)) {
                fs.mkdirSync(dir);
            }
            await page.screenshot({ path: `${dir}/screenshot${mm + "-" + dd + "-" + yyyy + "-" + today.getTime()}.png` });
        });

        // await page.setViewport({ width: 320, height: 600 });
        await page.setUserAgent('Mozilla/5.0 (iPhone; CPU iPhone OS 9_0_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13A404 Safari/601.1');
        await page.goto(url, {
            waitUntil: 'networkidle0',
            timeout: 3000000
        });

        await page.waitFor(3 * 1000);

        await page.waitFor(() => {
            const modal = document.querySelector("body.modal-open");
            if (modal) {
                const modalText = document.querySelector("body > div:nth-child(12) > div.fade.in.modal > div > div > div.modal-body > p");
                return modalText.textContent == "Sorry, there are no cars available.";
            }
            return !modal;
        });

        const result = await page.evaluate(() => {
            const ul = document.querySelector("ul.list-unstyled");
            const models = [];
            if (ul) {
                for (let index = 0; index < ul.childElementCount; index++) {
                    const li = ul.children[index];
                    const div = li.children[0];
                    let vehicle = "";
                    let totalPrice = "";
                    let transmission = "";
                    let type = "";

                    for (let i = 0; i < div.childElementCount; i++) {
                        const rowChildren = div.children[i];

                        const transmissionDiv = Array.from(rowChildren.children).find(
                            i => i.className === "vehicle-list-item__column transmission "
                        );
                        for (let j = 0; j < transmissionDiv.childElementCount; j++) {
                            const childElement = transmissionDiv.children[j];
                            if (childElement.nodeName === "BR") {
                                transmission = transmission + "\r\n";
                            } else if (
                                childElement.nodeName === "B" ||
                                childElement.nodeName === "SPAN"
                            ) {
                                transmission = transmission + childElement.innerHTML;
                            }
                        }

                        const PriceDiv = Array.from(rowChildren.children).find(
                            i => i.className === "vehicle-list-item__column price "
                        );
                        for (let j = 0; j < PriceDiv.childElementCount; j++) {
                            const childElement = PriceDiv.children[j];
                            if (childElement.nodeName === "BR") {
                                totalPrice = totalPrice + "\r\n";
                            } else if (
                                childElement.nodeName === "B" ||
                                childElement.nodeName === "SPAN"
                            ) {
                                totalPrice = totalPrice + childElement.innerHTML;
                            }
                        }
                        const nameDiv = Array.from(rowChildren.children).find(
                            i => i.className === "vehicle-list-item__column-name "
                        );
                        for (let j = 0; j < nameDiv.childElementCount; j++) {
                            const childElement = nameDiv.children[j];
                            if (childElement.nodeName === "BR") {
                                vehicle = vehicle + "\r\n";
                            } else if (
                                childElement.nodeName === "B" ||
                                childElement.nodeName === "SPAN"
                            ) {
                                vehicle = vehicle + childElement.innerHTML.replace(/<\!--.*?-->/g, "");
                            }
                        }
                        const typeDiv = Array.from(rowChildren.children).find(
                            i => i.className === "vehicle-list-item__column-info"
                        );
                        for (let j = 0; j < typeDiv.childElementCount; j++) {
                            const childElement = typeDiv.children[j];
                            if (childElement.nodeName === "BR") {
                                type = type + "\r\n";
                            } else if (
                                childElement.nodeName === "B" ||
                                childElement.nodeName === "SPAN"
                            ) {
                                type = type + childElement.innerHTML;
                            }
                        }
                    }

                    models.push({
                        vehicle: vehicle,
                        totalPrice: totalPrice,
                        type: type,
                        transmission: transmission
                    });
                }
            }
            return models;
        });

        const screenshotFolder = path.resolve(__dirname, "../ScreenShot");

        if (!fs.existsSync(screenshotFolder)) {
          fs.mkdirSync(screenshotFolder);
        }
  
        const dir = screenshotFolder + "/" + mm + "-" + dd + "-" + yyyy;
  
        if (!fs.existsSync(dir)) {
          fs.mkdirSync(dir);
        }

        await page.screenshot({
            path: `${dir}/${param.id}.png`,
            fullPage: true
        });
        await browser.close();
        console.log(result);
    } catch (err) {
        console.error(err);
    }
};

(async () => {
    try {
        await getData({
            location: "Sydney Airport, New South Wales, Australia",
            pickupDate: 1,
            returnDate: 8,
            id: 5
        });
    } catch (err) {
        console.error(err);
    }
})();