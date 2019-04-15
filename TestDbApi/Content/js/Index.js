var app = new Vue({
    el: '#app',
    data: {
        sortKey: 'Id',
        reverse: false,
        search: '',
        users: [],
        groups: [],
        newName: '',
        newBirthDay: '1992-02-29'
    },
    computed: {
        orderedUsers: function () {
            let rev = this.reverse ? 'desc' : 'asc';
            var self = this
            //Filter

            let result = self.users.filter(function (user) {
                let inString = user.Id + user.Name + user.BirthDay + user.GroupId;
                return new RegExp(self.search, 'i').test(inString);
            });
            //Sort
            result = _.orderBy(result, this.sortKey, rev);
            return result;
        }
    },
    methods: {
        sortBy: function (sortKey) {
            this.reverse = (this.sortKey == sortKey) ? !this.reverse : false;
            this.sortKey = sortKey;
        },

        addUser: function () {
            this.users.push(this.newUser);
            this.newUser = {};
        },
        deleteUser: function (id) {
            fetch('/api/v1/users/' + id + '/delete', { method: 'POST' })
                .then(r => {
                    if (r.ok) {
                        //Обновить список пользователей
                        UpdateUsersData();
                        ok('Пользователь успешно удален');
                    }
                });
        },
        addUser: function () {
            let newUserData = {
                Name: this.newName,
                BirthDay: this.newBirthDay,
                GroupId:1 //Пока что всех в одну группу
            }
            fetch('/api/v1/users/add', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newUserData)
            }).then((response) => {
                if (response.ok) {
                    UpdateUsersData();
                    ok('Пользователь успешно добавлен');
                } else { err('Ошибка сервера' + response.statusText); }
            });
            ok(this.newName + ' ' + this.newBirthDay);
        },
        updateUser: function (user) {
            fetch('/api/v1/users/' + user.id + '/update', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(user)
            }).then((response) => {
                if (response.ok) {
                    ok('Данные успешно изменены');
                } else { err('Ошибка сервера' + response.statusText); }
            });
        }
    }
});

function ok(text) {
    const Toast = Swal.mixin({
        toast: true,
        position: 'center',
        showConfirmButton: false,
        timer: 3000
    });

    Toast.fire({
        type: 'success',
        title: text
    })
}
function err(text) {
    const Toast = Swal.mixin({
        toast: true,
        position: 'center',
        showConfirmButton: false,
        timer: 3000
    });

    Toast.fire({
        type: 'error',
        title: text
    })
}

//Загружаем пользователей с сервера
function UpdateUsersData() {
    fetch('/api/v1/groups/0/users')
        .then(r => r.json())
        .then(data => app.users = data);
}
function UpdateGroupsList() {
    fetch('/api/v1/groups/0')
        .then(r => r.json())
        .then(data => app.groups = data);
}