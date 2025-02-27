import { useState, useEffect } from 'react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useUserContext } from "../../providers";
import { UserType, AllUserTypes, UserState, AllUserState } from "../../types";

type EditUserFormInputs = {
    name: string;
    type: string;
    state: string;
};

export const EditUser = () => {
    const { register, handleSubmit, formState: { errors }, setValue } = useForm<EditUserFormInputs>();
    const [errorMessage, setErrorMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();
    const { fetchUser, user } = useUserContext();

    useEffect(() => {
        if (user) {
            setValue('name', user.name);
            setValue('type', UserType[user.userType]);
            setValue('state', UserState[user.userState]);
        }
    }, [setValue]);

    const onSubmit: SubmitHandler<EditUserFormInputs> = async (data) => {
        setIsLoading(true);
        try {
            // サーバーにログインリクエストを送信
            await axios.post('/Api/User/Edit', data).then(() => {
                fetchUser();
                navigate('/PrivatePage');
            });
        } catch (error: any) {
            // エラーメッセージを表示
            console.error(error);
            setErrorMessage(error.response.data.message || 'エラーが発生しました。');
        } finally {
            setIsLoading(false);
        }
    };


    return (
        <div>
            <h2 className="pageTitle">Edit</h2>
            <div className="formWrapperBox">
                <form onSubmit={handleSubmit(onSubmit)}>
                    <dl>
                        <dt><label>Name</label></dt>
                        <dd>
                            <input type="text" {...register('name', {
                                required: "必須です。",
                                minLength: { value: 3, message: "名前は3文字以上です。" },
                                maxLength: { value: 30, message: "名前は30文字以下です。" },
                            })} />
                            <span>{errors.name?.message}</span>
                        </dd>
                        <dt><label>Type</label></dt>
                        <dd>
                            <select {...register('type', { required: '選択してください' })}>
                                {AllUserTypes.map(userType => (
                                    <option key={UserType[userType]} value={UserType[userType]}>
                                        {userType}
                                    </option>
                                ))}
                            </select>
                            <span>{errors.type?.message}</span>
                        </dd>
                        <dt><label>State</label></dt>
                        <dd>
                            <select {...register('state', { required: '選択してください' })}>
                                {AllUserState.map(userState => (
                                    <option key={UserState[userState]} value={UserState[userState]}>
                                        {userState}
                                    </option>
                                ))}
                            </select>
                            <span>{errors.type?.message}</span>
                        </dd>
                    </dl>
                    <div className="formErrorMassage">{errorMessage && <p>{errorMessage}</p>}</div>
                    <div className="formFooter"><button type="submit" disabled={isLoading}>Change</button></div>
                </form>
            </div>
        </div>
    );
}

export default EditUser;