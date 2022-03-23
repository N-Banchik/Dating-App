import{User} from './user'

export class UserParams {
    gender: 'male'|'female';
    minAge =18;
    maxAge = 150;
    pageNumber = 1;
    pageSize=5;
    orderBy='lastActive';

    constructor({Gender}:User) {
        
        this.gender = Gender ==='male'? 'female':'male'
    }
}
