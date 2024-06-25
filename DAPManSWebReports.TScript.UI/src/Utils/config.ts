import configData from '..//../appsettings.json';

interface Config {
    ApiBaseUrlDev: string;
    ApiBaseUrlRelease: string;
}

const config: Config = configData;

export default config;