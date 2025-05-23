import { environment } from "./environment";

const soundBase = `${environment.apiUrl}/sound`;
const userBase = `${environment.apiUrl}/user`;

export const api = {
    sound: {
        getSound: `${soundBase}/getSound`,

        getSoundByAdmin: `${soundBase}/getSoundByAdmin`,

        addSound: `${soundBase}/addSound`,

        updateSound: `${soundBase}/updateSound`,

        deleteSound: `${soundBase}/deleteSound`,

        activateSound: `${soundBase}/activeSound`,

        getSoundMix: `${soundBase}/getSoundMix`,

        createMix: `${soundBase}/createMix`,

        saveMix: `${soundBase}/saveMix`,
    },
    user: {
        getListUser: `${userBase}/getListUser`,

        createUser: `${userBase}/createUser`,

        updateUser: `${userBase}/updateUser`,

        deleteUser: `${userBase}/deleteUser`,

        activateUser: `${userBase}/activeSound`,

        login: `${userBase}/login`,

        createAdmin: `${userBase}/createAdmin`,

        updateRole: `${userBase}/updateRole`,

        getListRole: `${userBase}/getListRole`,

        changePassword: `${userBase}/changePassword`,

        forgotPassword: `${userBase}/forgotPassword`,

        changeForgotPassword: `${userBase}/changeForgotPassword`,
    },
}