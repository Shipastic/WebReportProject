import configData from '..//../appsettings.json';

interface Config {
    ApiBaseUrl: string;
}

const config: Config = configData;

export default config;